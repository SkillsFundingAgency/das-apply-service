using NUnit.Framework;
using FluentAssertions;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;
using System;
using SFA.DAS.ApplyService.Web.Validators;

namespace SFA.DAS.ApplyService.Web.UnitTests
{
    [TestFixture]
    public class TrusteeDateOfBirthValidatorTests
    {
        [Test]
        public void Validator_rejects_missing_date_of_birth_for_single_trustee()
        {
            var answers = new List<Answer>();
            var trusteeData = new TabularData
            {
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = "10001000",
                        Columns = new List<string>
                        {
                            "Ms A Trustee"
                        }
                    }
                }
            };

            var validationErrorDetails = TrusteeDateOfBirthValidator.ValidateTrusteeDatesOfBirth(trusteeData, answers);

            validationErrorDetails.Count.Should().Be(1);
            validationErrorDetails[0].ErrorMessage.Should().Be(DateOfBirthValidator.MissingDateOfBirthErrorMessage);
        }

        [Test]
        public void Validator_rejects_missing_date_of_birth_for_multiple_trustees()
        {
            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = "10002000_Month",
                    Value = "11"
                },
                new Answer
                {
                    QuestionId = "10002000_Year",
                    Value = "1980"
                }
            };
            var trusteeData = new TabularData
            {
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = "10002000",
                        Columns = new List<string>
                        {
                            "Ms A Trustee"
                        }
                    },
                    new TabularDataRow
                    {
                        Id = "10003000",
                        Columns = new List<string>
                        {
                            "Mr B Trustee"
                        }
                    }
                }
            };

            var validationErrorDetails = TrusteeDateOfBirthValidator.ValidateTrusteeDatesOfBirth(trusteeData, answers);

            validationErrorDetails.Count.Should().Be(1);
            validationErrorDetails[0].ErrorMessage.Should().Be(DateOfBirthValidator.MissingDateOfBirthErrorMessage);
        }

        [TestCase("0")]
        [TestCase("")]
        [TestCase("13")]
        public void Validator_rejects_invalid_month(string month)
        {
            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = "10001000_Month",
                    Value = month
                },
                new Answer
                {
                    QuestionId = "10001000_Year",
                    Value = "1980"
                }
            };
            var trusteeData = new TabularData
            {
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = "10001000",
                        Columns = new List<string>
                        {
                            "Ms A Trustee"
                        }
                    }
                }
            };

            var validationErrorDetails = TrusteeDateOfBirthValidator.ValidateTrusteeDatesOfBirth(trusteeData, answers);

            validationErrorDetails.Count.Should().Be(1);
            validationErrorDetails[0].ErrorMessage.Should().Be(DateOfBirthValidator.InvalidIncompleteDateOfBirthErrorMessage);
        }

        [TestCase("0")]
        [TestCase("")]
        public void Validator_rejects_invalid_year(string year)
        {
            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = "10001000_Month",
                    Value = "5"
                },
                new Answer
                {
                    QuestionId = "10001000_Year",
                    Value = year
                }
            };
            var trusteeData = new TabularData
            {
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = "10001000",
                        Columns = new List<string>
                        {
                            "Ms A Trustee"
                        }
                    }
                }
            };

            var validationErrorDetails = TrusteeDateOfBirthValidator.ValidateTrusteeDatesOfBirth(trusteeData, answers);

            validationErrorDetails.Count.Should().Be(1);
            validationErrorDetails[0].ErrorMessage.Should().Be(DateOfBirthValidator.InvalidIncompleteDateOfBirthErrorMessage);
        }

        [Test]
        public void Validator_rejects_year_in_future()
        {
            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = "10001000_Month",
                    Value = "5"
                },
                new Answer
                {
                    QuestionId = "10001000_Year",
                    Value = (DateTime.Now.Year + 1).ToString()
                }
            };
            var trusteeData = new TabularData
            {
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = "10001000",
                        Columns = new List<string>
                        {
                            "Ms A Trustee"
                        }
                    }
                }
            };

            var validationErrorDetails = TrusteeDateOfBirthValidator.ValidateTrusteeDatesOfBirth(trusteeData, answers);

            validationErrorDetails.Count.Should().Be(1);
            validationErrorDetails[0].ErrorMessage.Should().Be(DateOfBirthValidator.DateOfBirthInFutureErrorMessage);
        }

        [Test]
        public void Validator_rejects_current_month_and_year()
        {
            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = "10001000_Month",
                    Value = (DateTime.Now.Month).ToString()
                },
                new Answer
                {
                    QuestionId = "10001000_Year",
                    Value = (DateTime.Now.Year).ToString()
                }
            };
            var trusteeData = new TabularData
            {
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = "10001000",
                        Columns = new List<string>
                        {
                            "Ms A Trustee"
                        }
                    }
                }
            };

            var validationErrorDetails = TrusteeDateOfBirthValidator.ValidateTrusteeDatesOfBirth(trusteeData, answers);

            validationErrorDetails.Count.Should().Be(1);
            validationErrorDetails[0].ErrorMessage.Should().Be(DateOfBirthValidator.DateOfBirthInFutureErrorMessage);
        }

        [TestCase(1)]
        [TestCase(12)]
        [TestCase(123)]
        [TestCase(999)]
        public void Validator_rejects_year_with_less_than_4_digits(int year)
        {
            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = "10001000_Month",
                    Value = "5"
                },
                new Answer
                {
                    QuestionId = "10001000_Year",
                    Value = year.ToString()
                }
            };
            var trusteeData = new TabularData
            {
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = "10001000",
                        Columns = new List<string>
                        {
                            "Ms A Trustee"
                        }
                    }
                }
            };

            var validationErrorDetails = TrusteeDateOfBirthValidator.ValidateTrusteeDatesOfBirth(trusteeData, answers);

            validationErrorDetails.Count.Should().Be(1);
            validationErrorDetails[0].ErrorMessage.Should().Be(DateOfBirthValidator.DateOfBirthYearLengthErrorMessage);
        }
    }
}
