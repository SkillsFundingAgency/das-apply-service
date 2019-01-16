using Dapper;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Data;

namespace SFA.DAS.ApplyService.Data.DapperTypeHandlers
{
    public class SequenceDataHandler : SqlMapper.TypeHandler<SequenceData>
    {
        public override void SetValue(IDbDataParameter parameter, SequenceData value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }

        public override SequenceData Parse(object value)
        {
            return JsonConvert.DeserializeObject<SequenceData>(value.ToString());
        }
    }
}
