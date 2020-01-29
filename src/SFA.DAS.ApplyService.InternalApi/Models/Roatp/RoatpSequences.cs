
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Models.Roatp
{
    public class RoatpSequences
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Sequential { get; set; }

        public List<string> ExcludeSections { get; set; }
    }
   
}
