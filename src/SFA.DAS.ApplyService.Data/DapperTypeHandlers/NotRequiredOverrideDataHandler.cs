using Dapper;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using System.Data;

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
