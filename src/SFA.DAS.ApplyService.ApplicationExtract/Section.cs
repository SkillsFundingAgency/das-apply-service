using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApplyService.ApplicationExtract
{
    public class Section
    {
        public Section()
        {
            Questions = new List<Question>();
        }

        public string Title { get; set; }

        public List<Question> Questions { get; set; }
    }
}
