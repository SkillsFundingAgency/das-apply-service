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


            var version = await _applyRepository.GetVersionForApplication(applicationId);
            var sourceValue = GetIdentifierSourceValue(questionIdentifier, version);

            if (sourceValue?.Result?.Source == "qna")
            {
                return await GetAnswersForQuestion(sourceValue.Result?.Value, applicationId);
            }

            if (sourceValue?.Result?.Source == "table")
            {
                return await GetAnswersFromTable(sourceValue.Result?.Value, applicationId);
            }



            return null;
        }

        public async Task<string> GetAnswersFromTable(string tableAndField, Guid applicationId)
        {
            if (tableAndField.StartsWith("organisation-"))
            {
                var organisation = await _applyRepository.GetOrganisationForApplication(applicationId);
                if (tableAndField== "organisation-ukprn")
                    return organisation?.OrganisationUkprn.ToString();

                if (tableAndField.EndsWith("organisation-name"))
                    return organisation?.Name;

                if (tableAndField.EndsWith("organisation-type"))
                    return organisation?.OrganisationType;
            }
            
            return null;
        }

        public async Task<string> GetAnswersForQuestion(string questionId, Guid applicationId)
        {
            var sections = await _applyRepository.GetSections(applicationId);
            foreach (var section in sections)
            {
                foreach (var qna in section.QnAData.Pages)
                {
                    foreach (var page in qna.PageOfAnswers)
                    {
                        var answers =
                            page.Answers.Where(x => x.QuestionId == questionId ||
                                                    x.QuestionId.Contains($"{questionId}.")).ToList();
                        {
                            if (answers.Count() == 1)
                            {
                                {
                                    return answers.First().Value;
                                }
                            }
                            if (answers.Count() > 1)
                            {
                                var subAnswers =
                                    answers.Where(x => x.QuestionId.Contains($"{questionId}.")).ToList();
                                if (!subAnswers.Any())
                                {
                                    {
                                        return answers.First().Value;
                                    }
                                }
                                return subAnswers.Count() == 1 ? subAnswers.First().Value : string.Join(",", subAnswers);
                            }
                        }
                    }
                }
            }
            return null;
        }

        public async Task<IdentifierSourceValue?> GetIdentifierSourceValue(string identifier, string version)
        {
            // needs to go into some sort of table or declarative structure that's easy to release
            // and will need to get, from an identifier, the source, the value and the version

            if (version =="1.0")
            { 
                switch (identifier)
                {
                    case "trading-name":
                        return new IdentifierSourceValue { Source = "qna", Value = "CD-30"};
                    case "use-trading-name":
                        return new IdentifierSourceValue {Source = "qna", Value = "CD-01"};
                    case "company-number":
                        return new IdentifierSourceValue {Source = "qna", Value = "CD-17"};
                    case "charity-number":
                        return new IdentifierSourceValue { Source = "qna", Value = "CD-26" };
                    case "contact-name":
                        return new IdentifierSourceValue { Source = "qna", Value = "CD-02" };
                    case "contact-email":
                        return new IdentifierSourceValue { Source = "qna", Value = "CD-05" };
                    case "contact-phone-number":
                        return new IdentifierSourceValue { Source = "qna", Value = "CD-06" };
                    case "contact-address":
                        return new IdentifierSourceValue { Source = "qna", Value = "CD-03" };
                    case "contact-postcode":
                        return new IdentifierSourceValue { Source = "qna", Value = "CD-04" };
                    case "company-website":
                        return new IdentifierSourceValue { Source = "qna", Value = "CC-40" };
                    case "company-ukprn":
                        return new IdentifierSourceValue { Source = "qna", Value = "CD-12" };
                    case "organisation-ukprn":
                        return new IdentifierSourceValue { Source = "table", Value = "organisation-ukprn" };
                    case "organisation-name":
                        return new IdentifierSourceValue { Source = "table", Value = "organisation-name" };
                    case "organisation-type":
                        return new IdentifierSourceValue { Source = "table", Value = "organisation-type" };
                    
                }
            }

            return null;


        }
    }
}
