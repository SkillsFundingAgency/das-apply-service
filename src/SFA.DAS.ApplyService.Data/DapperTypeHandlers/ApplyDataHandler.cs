using System.Data;
using Dapper;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Data.DapperTypeHandlers
{
    public class ApplyDataHandler : SqlMapper.TypeHandler<ApplyData>
    {
        public override void SetValue(IDbDataParameter parameter, ApplyData value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }

        public override ApplyData Parse(object value)
        {
            return JsonConvert.DeserializeObject<ApplyData>(value.ToString());
        }
    }
}