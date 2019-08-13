using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Validation;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.UnitTests.Validators.MaxWordCountValidatorTests
{
    [TestFixture]
    public class When_Validate_called
    {
        [TestCase("", 10, true)]
        [TestCase("Mary had a little lamb", 10, true)]
        [TestCase("    Mary  had   a   little lamb ", 10, true)]
        [TestCase("Mary had a little lamb, its fleece was white as snow", 10, false)]
        [TestCase("   Mary had a     little lamb, its fleece was white as snow                   ", 10, false)]
        public void Then_correct_errors_are_returned(string input, long wordLimit, bool isValid)
        {
            var validator = new MaxWordCountValidator
            {
                ValidationDefinition = new ValidationDefinition()
                {
                    ErrorMessage = "Word count exceeded", 
                    Name = "WordCount", 
                    Value = wordLimit
                }
            };

            var questionId = "Q1";
            var errors = validator.Validate(questionId, new Answer() {Value = input, QuestionId = questionId });

            (errors.Count == 0).Should().Be(isValid);
        }
    }
}