namespace SFA.DAS.ApplyService.Application.Email.Consts
{
    public static class EmailTemplateName
    {
        /// <summary>
        /// Requires tokens: { contactname }
        /// </summary>
        public const string APPLY_SIGNUP_ERROR = "ApplySignupError";

        /// <summary>
        /// Requires tokens: { contactname }
        /// </summary>
        public const string APPLY_EPAO_UPDATE = "ApplyEPAOUpdate";

        /// <summary>
        /// Requires tokens: { contactname, standard }
        /// </summary>
        public const string APPLY_EPAO_RESPONSE = "ApplyEPAOResponse";

        /// <summary>
        /// Requires tokens: { contactname, reference }
        /// </summary>
        public const string APPLY_EPAO_INITIAL_SUBMISSION = "ApplyEPAOInitialSubmission";

        /// <summary>
        /// Requires tokens: { contactname, standard, reference }
        /// </summary>
        public const string APPLY_EPAO_STANDARD_SUBMISSION = "ApplyEPAOStandardSubmission";

        /// <summary>
        /// Requires tokens: { ApplicantFullName, UKPRN, OrganisationName, ApplicationSequence, ApplicationSection, PageTitle, GetHelpQuery }
        /// </summary>
        public const string ROATP_GET_HELP_WITH_QUESTION = "RoATPGetHelpWithQuestion";
    }
}
