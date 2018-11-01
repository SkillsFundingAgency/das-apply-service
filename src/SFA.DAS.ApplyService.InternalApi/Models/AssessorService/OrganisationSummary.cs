namespace SFA.DAS.ApplyService.InternalApi.Models.AssessorService
{
    public class OrganisationSummary
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long? Ukprn { get; set; }

        public OrganisationData OrganisationData { get; set; }

        public int? OrganisationTypeId { get; set; }
        public string OrganisationType { get; set; }
    }
}
