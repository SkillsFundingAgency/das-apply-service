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
    }
}
