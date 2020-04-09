using Dapper;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Data;

namespace SFA.DAS.ApplyService.Data.DapperTypeHandlers
{
    public class FinancialReviewDetailsDataHandler : SqlMapper.TypeHandler<FinancialReviewDetails>
    {
        public override void SetValue(IDbDataParameter parameter, FinancialReviewDetails value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }

        public override FinancialReviewDetails Parse(object value)
        {
            return JsonConvert.DeserializeObject<FinancialReviewDetails>(value.ToString());
        }

    }
}
