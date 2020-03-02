using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class AnswerFormService : IAnswerFormService
    {
        public List<Answer> GetAnswersFromForm(HttpContext httpContext)
        {
            var answers = new List<Answer>();

            // These are special in that they drive other things and thus should not be deemed as an answer
            var exludedInputs = new List<string> { "redirectAction", "postcodeSearch", "checkAll" };

            Dictionary<string, JObject> answerValues = new Dictionary<string, JObject>();

            foreach (var formVariable in httpContext.Request.Form.Where(f => !f.Key.StartsWith("__") && !exludedInputs.Contains(f.Key, StringComparer.InvariantCultureIgnoreCase)))
            {
                var answerKey = formVariable.Key.Split("_Key_");
                if (!answerValues.ContainsKey(answerKey[0]))
                {
                    answerValues.Add(answerKey[0], new JObject());
                }

                answerValues[answerKey[0]].Add(
                    answerKey.Count() == 1 ? string.Empty : answerKey[1],
                    formVariable.Value.ToString());
            }

            foreach (var answer in answerValues)
            {
                if (answer.Value.Count > 1)
                {
                    answers.Add(new Answer() { QuestionId = answer.Key, JsonValue = answer.Value });
                }
                else
                {
                    answers.Add(new Answer() { QuestionId = answer.Key, Value = answer.Value.Value<string>(string.Empty) });
                }
            }

            return answers;
        }
    }
}
