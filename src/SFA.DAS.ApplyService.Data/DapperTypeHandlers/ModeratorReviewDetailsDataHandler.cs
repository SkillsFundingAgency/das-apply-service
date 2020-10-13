using System.Data;
using Dapper;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Data.DapperTypeHandlers
{
    public class ModeratorReviewDetailsDataHandler : SqlMapper.TypeHandler<ModeratorReviewDetails>
    {
        public override void SetValue(IDbDataParameter parameter, ModeratorReviewDetails value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }

        public override ModeratorReviewDetails Parse(object value)
        {
            return JsonConvert.DeserializeObject<ModeratorReviewDetails>(value.ToString());
        }

    }
}
