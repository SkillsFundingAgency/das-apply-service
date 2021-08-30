using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http.Internal;
using NUnit.Framework;
using SFA.DAS.ApplyService.Web.Validators.Appeals;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals;

namespace SFA.DAS.ApplyService.Web.UnitTests.Validators.Appeals
{
    [TestFixture]
    public class GroundsOfAppealViewModelValidatorTests
    {
        private GroundsOfAppealViewModelValidator _validator;

        private GroundsOfAppealViewModel _viewModel;

        [SetUp]
        public void Arrange()
        {
            _validator = new GroundsOfAppealViewModelValidator();

            _viewModel = new GroundsOfAppealViewModel
            {
                ApplicationId = Guid.NewGuid()
            };
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public void Validate_Returns_Valid_When_Appeal_Options_Are_Selected_and_Their_Reason_Entered(bool appealOnPolicyOrProcesses, bool appealOnEvidenceSubmitted)
        {
            var reasonText = string.Concat(Enumerable.Repeat("a", GroundsOfAppealViewModelValidator.MaxLength));

            _viewModel.AppealOnPolicyOrProcesses = appealOnPolicyOrProcesses;
            _viewModel.HowFailedOnPolicyOrProcesses = appealOnPolicyOrProcesses ? reasonText : null;

            _viewModel.AppealOnEvidenceSubmitted = appealOnEvidenceSubmitted;
            _viewModel.HowFailedOnEvidenceSubmitted = appealOnEvidenceSubmitted ? reasonText : null;

            var result = _validator.Validate(_viewModel);

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_HowFailedOnPolicyOrProcesses_is_Missing()
        {
            _viewModel.AppealOnPolicyOrProcesses = true;
            _viewModel.HowFailedOnPolicyOrProcesses = null;

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.HowFailedOnPolicyOrProcesses), result.Errors[0].PropertyName);
            Assert.AreEqual(GroundsOfAppealViewModelValidator.NoHowFailedOnPolicyOrProcessesEntered, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_HowFailedOnPolicyOrProcesses_is_Too_Long()
        {
            _viewModel.AppealOnPolicyOrProcesses = true;
            _viewModel.HowFailedOnPolicyOrProcesses = string.Concat(Enumerable.Repeat("a", GroundsOfAppealViewModelValidator.MaxLength + 1));

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.HowFailedOnPolicyOrProcesses), result.Errors[0].PropertyName);
            Assert.AreEqual(GroundsOfAppealViewModelValidator.MaxLengthError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_HowFailedOnEvidenceSubmitted_is_Missing()
        {
            _viewModel.AppealOnEvidenceSubmitted = true;
            _viewModel.HowFailedOnEvidenceSubmitted = null;

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.HowFailedOnEvidenceSubmitted), result.Errors[0].PropertyName);
            Assert.AreEqual(GroundsOfAppealViewModelValidator.NoHowFailedOnEvidenceSubmittedEntered, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_HowFailedOnEvidenceSubmitted_is_Too_Long()
        {
            _viewModel.AppealOnEvidenceSubmitted = true;
            _viewModel.HowFailedOnEvidenceSubmitted = string.Concat(Enumerable.Repeat("a", GroundsOfAppealViewModelValidator.MaxLength + 1));

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.HowFailedOnEvidenceSubmitted), result.Errors[0].PropertyName);
            Assert.AreEqual(GroundsOfAppealViewModelValidator.MaxLengthError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Valid_When_UPLOAD_APPEALFILE_FORMACTION_and_AppealFileToUpload_is_Valid_PDF()
        {
            _viewModel.FormAction = GroundsOfAppealViewModel.UPLOAD_APPEALFILE_FORMACTION;
            _viewModel.AppealFileToUpload = GenerateAppealFileToUpload("file.pdf", true, GroundsOfAppealViewModelValidator.MaxFileSizeInBytes);

            var result = _validator.Validate(_viewModel);

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_UPLOAD_APPEALFILE_FORMACTION_and_AppealFileToUpload_is_Missing()
        {
            _viewModel.FormAction = GroundsOfAppealViewModel.UPLOAD_APPEALFILE_FORMACTION;
            _viewModel.AppealFileToUpload = null;

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.AppealFileToUpload), result.Errors[0].PropertyName);
            Assert.AreEqual(GroundsOfAppealViewModelValidator.AppealFileRequired, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_AppealFileToUpload_is_Not_PDF()
        {
            _viewModel.FormAction = GroundsOfAppealViewModel.UPLOAD_APPEALFILE_FORMACTION;
            _viewModel.AppealFileToUpload = GenerateAppealFileToUpload("file.txt", false, GroundsOfAppealViewModelValidator.MaxFileSizeInBytes);

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.AppealFileToUpload), result.Errors[0].PropertyName);
            Assert.AreEqual(GroundsOfAppealViewModelValidator.FileMustBePdf, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_AppealFileToUpload_Exceeds_MaxFileSize()
        {
            _viewModel.FormAction = GroundsOfAppealViewModel.UPLOAD_APPEALFILE_FORMACTION;
            _viewModel.AppealFileToUpload = GenerateAppealFileToUpload("file.pdf", true, GroundsOfAppealViewModelValidator.MaxFileSizeInBytes + 1);

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.AppealFileToUpload), result.Errors[0].PropertyName);
            Assert.AreEqual(GroundsOfAppealViewModelValidator.MaxFileSizeExceeded, result.Errors[0].ErrorMessage);
        }

        private static FormFile GenerateAppealFileToUpload(string fileName, bool hasPdfHeader, int length)
        {
            var pdfHeader = new byte[] { 0x25, 0x50, 0x44, 0x46 };

            MemoryStream fileContent = new MemoryStream();

            if (hasPdfHeader)
            {
                fileContent.Write(pdfHeader);
            }

            var remainingContentToGenerate = length - (int)fileContent.Length;

            if (remainingContentToGenerate > 0)
            {
                var contentToGenerate = Enumerable.Repeat((byte)0x20, remainingContentToGenerate);
                fileContent.Write(contentToGenerate.ToArray());
            }

            return new FormFile(fileContent, 0, fileContent.Length, fileName, fileName);
        }
    }
}
