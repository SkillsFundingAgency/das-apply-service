using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.ApplyService.Application.DataFeeds
{
    public class DataFeedFactory : IDataFeedFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DataFeedFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public IDataFeed GetDataField(string dataFeedName)
        {
            var dataFeed = _serviceProvider.GetServices<IDataFeed>()
                .FirstOrDefault(v => v.GetType().Name == dataFeedName + "DataFeed");

            return dataFeed;
        }
    }

    public interface IDataFeedFactory
    {
        IDataFeed GetDataField(string dataFeedName);
    }
}