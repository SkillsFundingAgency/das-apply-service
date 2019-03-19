using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Validation;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.UnitTests.Validators.MaxWordCountValidatorTests
{
    [TestFixture]
    public class When_Validate_called
    {
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

            var errors = validator.Validate(new Question(), new Answer() {Value = input, QuestionId = "Q1"});

            (errors.Count == 0).Should().Be(isValid);
        }
    }
}