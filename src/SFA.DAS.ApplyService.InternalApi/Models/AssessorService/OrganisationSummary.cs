namespace SFA.DAS.ApplyService.InternalApi.Models.AssessorService
{
    public class OrganisationSummary
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int? Ukprn { get; set; }
        public string Email { get; set; }

        public OrganisationData OrganisationData { get; set; }

        public string OrganisationType { get; set; }
    }
}
