using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class ApplicationSection : EntityBase
    {
        public Guid ApplicationId { get; set; }
        public int SectionId { get; set; }
        public int SequenceId { get; set; }
        public string QnAData { get; set; }
        
        public List<Page> Pages
        {
            get => JsonConvert.DeserializeObject<List<Page>>(QnAData);
            set => QnAData = JsonConvert.SerializeObject(value);
        }

        public string Title { get; set; }
        public string LinkTitle { get; set; }
        public string DisplayType { get; set; }
    }
}