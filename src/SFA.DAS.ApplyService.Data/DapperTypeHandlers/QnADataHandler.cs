using System.Data;
using Dapper;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Data.DapperTypeHandlers
{
    public class QnADataHandler : SqlMapper.TypeHandler<QnAData>
    {
        public override QnAData Parse(object value)
        {
            return JsonConvert.DeserializeObject<QnAData>(value.ToString());
        }

        public override void SetValue(IDbDataParameter parameter, QnAData value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }
    }
}
