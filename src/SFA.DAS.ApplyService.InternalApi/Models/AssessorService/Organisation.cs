namespace SFA.DAS.ApplyService.InternalApi.Models.AssessorService
{
    public class Organisation
    {
        public string EndPointAssessorOrganisationId { get; set; }
        public string EndPointAssessorName { get; set; }
        public int? EndPointAssessorUkprn { get; set; }
        public string Status { get; set; }
        public OrganisationData OrganisationData { get; set; }
    }
}
