using Dapper;
using Newtonsoft.Json;
using System.Data;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Data.DapperTypeHandlers
{
    public class NotRequiredOverrideDataHandler : SqlMapper.TypeHandler<NotRequiredOverrideConfiguration>
    {
        public override NotRequiredOverrideConfiguration Parse(object value)
        {
            return JsonConvert.DeserializeObject<NotRequiredOverrideConfiguration>(value.ToString());
        }

        public override void SetValue(IDbDataParameter parameter, NotRequiredOverrideConfiguration value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }
    }
}
