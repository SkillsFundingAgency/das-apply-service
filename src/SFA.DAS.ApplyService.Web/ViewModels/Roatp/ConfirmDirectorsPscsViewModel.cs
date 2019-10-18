using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConfirmDirectorsPscsViewModel
    {
        public Guid ApplicationId { get; set; }
        public PeopleInControl CompaniesHouseDirectors { get; set; }
        public PeopleInControl CompaniesHousePscs { get; set; }
    }

    public class PeopleInControl
    {
        public string QuestionId { get; set; }
        public dynamic TableData { get; set; }
    }
}
