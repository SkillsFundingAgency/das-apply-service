using Moq;
using Moq.Language.Flow;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests
{
    public static class MoqExtensions
    {
        public static void ReturnsInOrder<TMock, TResult>(this ISetup<TMock, TResult> setup, params TResult[] results) where TMock : class
        {
            // For each subsequent call to the mock, it will return the next result.
            setup.Returns(new Queue<TResult>(results).Dequeue);
        }

        public static void ReturnsInOrderAsync<TMock, TResult>(this ISetup<TMock, Task<TResult>> setup, params TResult[] results) where TMock : class
        {
            // For each subsequent call to the mock, it will get return next result asynchronously.
            setup.ReturnsAsync(new Queue<TResult>(results).Dequeue);
        }
    }
}
