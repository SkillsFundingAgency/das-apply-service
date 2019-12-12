using System.Data;
using Dapper;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Data.DapperTypeHandlers
{
    public class RoatpApplicationDataHandler : SqlMapper.TypeHandler<RoatpApplicationData>
    {
        public override void SetValue(IDbDataParameter parameter, RoatpApplicationData value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }

        public override RoatpApplicationData Parse(object value)
        {
            return JsonConvert.DeserializeObject<RoatpApplicationData>(value.ToString());
        }
        
    }
}
