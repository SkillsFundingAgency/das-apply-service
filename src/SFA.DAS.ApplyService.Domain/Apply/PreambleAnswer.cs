
namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class PreambleAnswer : Answer
    {
        private const int PreambleSequence = 0;
        
        public int SequenceId { get; set; }

        public PreambleAnswer()
        {
            SequenceId = PreambleSequence;
        }
    }
}
