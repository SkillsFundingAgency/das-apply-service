﻿namespace SFA.DAS.ApplyService.EmailService.Consts
{
    public static class EmailTemplateName
    {
        /// <summary>
        /// Requires tokens: { ApplicantEmail, ApplicantFullName, UKPRN, OrganisationName, ApplicationSequence, ApplicationSection, PageTitle, GetHelpQuery }
        /// </summary>
        public const string ROATP_GET_HELP_WITH_QUESTION = "RoATPGetHelpWithQuestion";

        /// <summary>
        /// Requires tokens: { ApplicantEmail, ApplicantFullName, ApplicationReferenceNumber }
        /// </summary>
        public const string ROATP_APPLICATION_SUBMITTED = "RoATPApplicationSubmitted";

        /// <summary>
        /// Requires tokens: { ApplicantFullName, LoginLink }
        /// </summary>
        public const string ROATP_APPLICATION_UPDATED = "RoATPApplicationUpdated";
    }
}
