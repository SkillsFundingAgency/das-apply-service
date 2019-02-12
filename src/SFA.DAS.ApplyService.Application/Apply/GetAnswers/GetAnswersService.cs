using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.GetAnswers
{

    public class GetAnswersService : IGetAnswersService
    {

        private readonly IApplyRepository _applyRepository;

        public GetAnswersService(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }


        public async Task<string> GetAnswers(string questionIdentifier, Guid applicationId)
        {
            // get workflow version for this application from workflows
            var sourceValue = GetIdentifierSourceValue(questionIdentifier);


            // MFCMFC make this not null
            if (sourceValue?.Result?.Source =="organisations")
            {
                return await GetAnswersFromTable(sourceValue.Result?.Source, sourceValue.Result?.Value,
                    applicationId);
            }

           // if (sourceValue?.Result?.Source == "qna")
            //{
                return await GetAnswersForQuestion(questionIdentifier, applicationId);
            //}

            return null;
        }

        public async Task<string> GetAnswersFromTable(string table, string field, Guid applicationId)
        {
            if (table == "organisations")
            {
                //var organisation = await _applyRepository.GetOrganisationForApplication(applicationId);
                //if (field== "OrganisationUkprn")
                //    return organisation?.OrganisationUkprn.ToString();

                //if (field== "Name")
                //    return organisation?.Name;

                //if (field == "OrganisationType")
                //    return organisation?.OrganisationType;
                return await _applyRepository.GetFieldValueFromOrganisation(applicationId, field);
            }

            return null;
        }

        public async Task<string> GetAnswersForQuestion(string answerTag, Guid applicationId)
        {
            var sections = await _applyRepository.GetSections(applicationId);
            foreach (var section in sections)
            {
                foreach (var qna in section.QnAData.Pages)
                {
                    if (qna.Questions.Any(x => x.AnswerTag == answerTag))
                    {
                        var questionId = qna.Questions.FirstOrDefault(x => x.AnswerTag == answerTag)?.QuestionId;
                        if (questionId==null) return null;
                        foreach (var page in qna.PageOfAnswers)
                        {
                            var answers =
                                page.Answers.Where(x => x.QuestionId == questionId ||
                                                        x.QuestionId.Contains($"{questionId}.")).ToList();
                            {
                                if (answers.Count == 1) return answers.First().Value;

                                if (answers.Count > 1)
                                {
                                    var subAnswers =
                                        answers.Where(x => x.QuestionId.Contains($"{questionId}.")).ToList();
                                    if (!subAnswers.Any())
                                    {
                                        {
                                            return answers.First().Value;
                                        }
                                    }
                                    return subAnswers.Count == 1 ? subAnswers.First().Value : string.Join(",", subAnswers);
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        public async Task<IdentifierSourceValue?> GetIdentifierSourceValue(string identifier)
        {
            // needs to go into some sort of table or declarative structure that's easy to release
            // and will need to get, from an identifier, the source, the value and the version

            switch (identifier)
            {
                case "trading-name":
                    return new IdentifierSourceValue { Source = "qna", Value = "CD-30" };
                //case "use-trading-name":
                //    return new IdentifierSourceValue {Source = "qna", Value = "CD-01"};
                //case "company-number":
                //    return new IdentifierSourceValue {Source = "qna", Value = "CD-17"};
                //case "charity-number":
                //    return new IdentifierSourceValue { Source = "qna", Value = "CD-26" };
                //case "contact-name":
                //    return new IdentifierSourceValue { Source = "qna", Value = "CD-02" };
                //case "contact-email":
                //    return new IdentifierSourceValue { Source = "qna", Value = "CD-05" };
                //case "contact-phone-number":
                //    return new IdentifierSourceValue { Source = "qna", Value = "CD-06" };
                //case "contact-address":
                //    return new IdentifierSourceValue { Source = "qna", Value = "CD-03" };
                //case "contact-postcode":
                //    return new IdentifierSourceValue { Source = "qna", Value = "CD-04" };
                //case "company-website":
                //    return new IdentifierSourceValue { Source = "qna", Value = "CC-40" };
                //case "company-ukprn":
                //    return new IdentifierSourceValue { Source = "qna", Value = "CD-12" };
                case "organisation-ukprn":
                    return new IdentifierSourceValue
                    {
                        Source = "organisations",
                        Value = "OrganisationUkprn"
                    };
                case "organisation-name":
                    return new IdentifierSourceValue { Source = "organisations", Value = "Name" };
                case "organisation-type":
                    return new IdentifierSourceValue
                    {
                        Source = "organisations",
                        Value = "OrganisationType"
                    };
                default:
                    return null;
            }
        }

    }
}
