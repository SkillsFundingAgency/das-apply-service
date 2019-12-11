
namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class PreambleAnswer : Answer
    {
        private const int PreambleSequence = 0;
        private const int PreambleSection = 1;
        private const string PreamblePage = "1";

        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get; set; }

        public PreambleAnswer()
        {
            SequenceId = PreambleSequence;
            SectionId = PreambleSection;
            PageId = PreamblePage;
        }
    }
}
