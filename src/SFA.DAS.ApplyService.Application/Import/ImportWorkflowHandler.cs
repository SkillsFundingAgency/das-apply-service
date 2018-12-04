using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Import
{
    public class ImportWorkflowHandler : IRequestHandler<ImportWorkflowRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public ImportWorkflowHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<Unit> Handle(ImportWorkflowRequest request, CancellationToken cancellationToken)
        {
            var workflowId = await _applyRepository.CreateNewWorkflow("EPAO");

            var spreadsheetRows =
                LoadSpreadsheetRows(request.ImportFile.OpenReadStream(),
                    "Technical Q&A format Developmen");


            AddAssets(spreadsheetRows);

            var sequences = spreadsheetRows.Select(sr => sr.SequenceId).Distinct().ToList();

            for (var sequenceIndex = 0; sequenceIndex < sequences.Count; sequenceIndex++)
            {
                var sequenceId = sequences[sequenceIndex];
                
                await _applyRepository.CreateSequence(workflowId, sequenceId, isActive: sequenceIndex == 0);

                var questionRows = spreadsheetRows
                    .Where(sr => sr.SequenceId == sequenceId && sr.Reference.Contains("SECTIONHEADER")).ToList();

                var sections = questionRows
                    .Select(sr => new WorkflowSection()
                    {
                        DisplayType = sr.SectionDisplayType,
                        LinkTitle = sr.SectionTitle,
                        SectionId = (int) sr.SectionId,
                        Title = sr.SectionTitle,
                        SequenceId = (int) sequenceId,
                        Status = "Draft",
                        WorkflowId = workflowId
                    })
                    .ToList();

                foreach (var section in sections)
                {
                    var pages = ListOfPagesForSection(spreadsheetRows, section);

                    foreach (var page in pages)
                    {
                        var questions = ListOfQuestionsForPage(spreadsheetRows, page, section.SectionId);

                        page.Questions = questions.Where(q => !q.QuestionId.Contains(".")).ToList();
                    }

                    section.QnAData = JsonConvert.SerializeObject(pages);

                    await _applyRepository.CreateSection(section);
                }
            }

            return new Unit();
        }

        private Question ProcessQuestion(QuestionRow spreadsheetQuestion, List<QuestionRow> page)
        {
            var question = new Question();
            question.QuestionId = spreadsheetQuestion.Reference;
            question.Label = spreadsheetQuestion.QuestionTextRef;
            question.Input = new Input()
            {
                Type = spreadsheetQuestion.QuestionType,
                Validations = spreadsheetQuestion.Validations.Select(q => new ValidationDefinition()
                    {Name = q, ErrorMessage = spreadsheetQuestion.ErrorMessage}).ToList()
            };

            if (spreadsheetQuestion.QuestionType.Contains("Radio"))
            {
                var options = new List<dynamic>();

                foreach (var radioOption in spreadsheetQuestion.RadioOptions)
                {
                    var furtherQuestions = page.Where(p =>
                            p.Reference.StartsWith(spreadsheetQuestion.Reference)
                            && p.Reference.LastIndexOf(".", StringComparison.InvariantCultureIgnoreCase) >
                            spreadsheetQuestion.Reference.IndexOf(".", StringComparison.InvariantCultureIgnoreCase)
                            && p.Choice == radioOption)
                        .ToList();

                    var fq = new List<Question>();
                    if (furtherQuestions.Any())
                    {
                        foreach (var furtherQuestion in furtherQuestions)
                        {
                            fq.Add(ProcessQuestion(furtherQuestion, page));
                        }
                    }

                    var option = new
                    {
                        Label = radioOption.Trim(),
                        Value = radioOption.Trim(),
                        FurtherQuestions = fq.Any() ? fq.ToArray() : null
                    };

                    options.Add(option);
                }

                question.Input.Options = options.ToArray();
            }

            return question;
        }

        private List<Question> ListOfQuestionsForPage(List<QuestionRow> spreadsheetRows, Page page, int sectionId)
        {
            var spreadsheetQuestions = spreadsheetRows
                .Where(r => r.PageNumber.ToString() == page.PageId &&
                            r.SectionId == sectionId &&
                            r.SequenceId.ToString() == page.SequenceId
                            && !string.IsNullOrWhiteSpace(r.QuestionType))
                .ToList();

            var questions = new List<Question>();

            foreach (var spreadsheetQuestion in spreadsheetQuestions)
            {
                var question = ProcessQuestion(spreadsheetQuestion, spreadsheetQuestions);

                questions.Add(question);
            }

            return questions;
        }

        private List<Page> ListOfPagesForSection(List<QuestionRow> spreadsheetRows, WorkflowSection section)
        {
            var pages = spreadsheetRows
                .Where(r => r.SectionId == section.SectionId &&
                            r.SequenceId == section.SequenceId &&
                            string.IsNullOrWhiteSpace(r.QuestionType) && r.PageNumber > 0)
                .Select(r => new Page
                {
                    PageId = r.PageNumber.ToString(),
                    Title = r.PageHeaderRef,
                    LinkTitle = r.PageLinkRef,
                    Visible = !r.HiddenByDefault,
                    Active = !r.HiddenByDefault,
                    PageOfAnswers = new List<PageOfAnswers>(),
                    AllowMultipleAnswers = r.AllowMultipleAnswers,
                    SequenceId = r.SequenceId.ToString(),
                    SectionId = r.SectionId.ToString(),
                    BodyText = r.BodyTextRef
                })
                .Distinct().ToList();

            for (int i = 0; i < pages.Count; i++)
            {
                var page = pages[i];

                page.Next = new List<Next>();
                if (i < pages.Count - 1)
                {
                    var spreadsheetRow = spreadsheetRows.First(r =>
                        r.PageNumber.ToString() == page.PageId && r.SectionId == section.SectionId &&
                        string.IsNullOrWhiteSpace(r.QuestionType));

                    if (!string.IsNullOrWhiteSpace(spreadsheetRow.Activate))
                    {
                        var conditions =
                            spreadsheetRow.Activate.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var condition in conditions)
                        {
                            var questionId = condition.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)[0];
                            var answerValue = condition.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)[1];
                            var pageId = condition.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)[2];

                            page.Next.Add(new Next()
                            {
                                Action = "NextPage", ReturnId = pageId,
                                Condition = new Condition() {MustEqual = answerValue, QuestionId = questionId}
                            });
                        }
                    }
                    else
                    {
                        page.Next.Add(new Next() {Action = "NextPage", ReturnId = pages[i + 1].PageId});
                    }
                }
                else
                {
                    page.Next.Add(new Next() {Action = "ReturnToSection", ReturnId = page.SectionId});
                }
            }

            return pages;
        }


        private void AddAssets(List<QuestionRow> spreadsheetRows)
        {
            var assets = new Dictionary<string, string>();

            foreach (var spreadsheetRow in spreadsheetRows)
            {
                AddAsset(assets, spreadsheetRow.PageLinkRef, spreadsheetRow.PageLinkTitle);
                AddAsset(assets, spreadsheetRow.PageHeaderRef, spreadsheetRow.PageHeader);
                AddAsset(assets, spreadsheetRow.QuestionTextRef, spreadsheetRow.QuestionText);
                AddAsset(assets, spreadsheetRow.ShortQuestionTextRef, spreadsheetRow.ShortQuestionText);
                AddAsset(assets, spreadsheetRow.BodyTextRef, spreadsheetRow.BodyText);
            }

            _applyRepository.AddAssets(assets);
        }

        private void AddAsset(Dictionary<string, string> assets, string reference, string text)
        {
            if (!String.IsNullOrWhiteSpace(reference))
            {
                assets.Add(reference, text);
            }
        }

        private List<QuestionRow> LoadSpreadsheetRows(Stream spreadsheetStream, string worksheetName)
        {
            var workbook =
                new XSSFWorkbook(spreadsheetStream);

            var sheet = workbook.GetSheet(worksheetName);

            var spreadsheetRows = new List<QuestionRow>();

            for (int i = 1; i < sheet.LastRowNum; i++)
            {

                IRow row = sheet.GetRow(i);

                if (row == null) continue;
                
                var questionRow = ReadQuestionRow(row, i+1);

                if (string.IsNullOrWhiteSpace(questionRow.SectionTitle)) continue;

                spreadsheetRows.Add(questionRow);
            }

            return spreadsheetRows;
        }

        private QuestionRow ReadQuestionRow(IRow row, int rowNumber)
        {
            var questionRow = new QuestionRow();
            try
            {
                questionRow.SequenceId = row.GetCell(0).NumericCellValue;
                questionRow.SectionId = row.GetCell(1).NumericCellValue;
                questionRow.SectionTitle = row.GetCell(2).StringCellValue;
                questionRow.PageNumber = row.GetCell(3).NumericCellValue;
                questionRow.Reference = row.GetCell(4)?.StringCellValue;
                questionRow.PageLinkTitle = row.GetCell(5)?.StringCellValue;
                questionRow.PageLinkRef = row.GetCell(6)?.StringCellValue;
                questionRow.PageHeader = row.GetCell(7)?.StringCellValue;
                questionRow.PageHeaderRef = row.GetCell(8)?.StringCellValue;
                questionRow.ShortQuestionText = row.GetCell(9)?.StringCellValue;
                questionRow.ShortQuestionTextRef = row.GetCell(10)?.StringCellValue;
                questionRow.QuestionText = row.GetCell(11)?.StringCellValue.Trim();
                questionRow.QuestionTextRef = row.GetCell(12)?.StringCellValue;
                questionRow.ErrorMessage = row.GetCell(13)?.StringCellValue;
                questionRow.ErrorMessageOption = row.GetCell(14)?.StringCellValue;
                questionRow.BodyText = row.GetCell(15)?.StringCellValue;
                questionRow.BodyTextRef = row.GetCell(16)?.StringCellValue;
                questionRow.HiddenByDefault = row.GetCell(17)?.StringCellValue == "Yes";
                questionRow.Activate = row.GetCell(18)?.StringCellValue;
                questionRow.AllowMultipleAnswers = row.GetCell(19)?.StringCellValue == "Yes";
                questionRow.QuestionType = row.GetCell(20)?.StringCellValue;
                questionRow.RadioOptions = row.GetCell(21)?.StringCellValue
                    .Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
                questionRow.Choice = row.GetCell(22)?.StringCellValue;
                questionRow.Validations = row.GetCell(23)?.StringCellValue
                    .Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
                questionRow.Guidance = row.GetCell(24)?.StringCellValue;
                questionRow.PromptReference = row.GetCell(25)?.StringCellValue;
                questionRow.PromptText = row.GetCell(26)?.StringCellValue;
                questionRow.PromptType = row.GetCell(27)?.StringCellValue;
                questionRow.ExcludeOrgTypes = row.GetCell(28)?.StringCellValue
                    .Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
                questionRow.SectionDisplayType = row.GetCell(29)?.StringCellValue;
            }
            catch (Exception e)
            {
                Console.Write($"Error dealing with row [{rowNumber}]: Message: {e.Message}");
            }


            return questionRow;
        }

        private class QuestionRow
        {
            public string SectionTitle { get; set; }
            public double PageNumber { get; set; }
            public string Reference { get; set; }
            public string PageHeader { get; set; }
            public string QuestionText { get; set; }
            public string QuestionType { get; set; }
            public string[] RadioOptions { get; set; }
            public string Choice { get; set; }
            public string[] Validations { get; set; }
            public string Guidance { get; set; }
            public string PromptReference { get; set; }
            public string PromptText { get; set; }
            public string PromptType { get; set; }
            public string[] ExcludeOrgTypes { get; set; }
            public double SectionId { get; set; }
            public bool AllowMultipleAnswers { get; set; }
            public string PageLinkTitle { get; set; }
            public string ShortQuestionText { get; set; }
            public string PageLinkRef { get; set; }
            public string PageHeaderRef { get; set; }
            public string ShortQuestionTextRef { get; set; }
            public string QuestionTextRef { get; set; }
            public bool HiddenByDefault { get; set; }
            public string Activate { get; set; }
            public bool SequenceDisabledByDefault { get; set; }
            public string[] ActivatesWhenComplete { get; set; }
            public string SectionDisplayType { get; set; }
            public double SequenceId { get; set; }
            public string BodyText { get; set; }
            public string BodyTextRef { get; set; }
            public string ErrorMessage { get; set; }
            public string ErrorMessageOption { get; set; }
        }
    }
}