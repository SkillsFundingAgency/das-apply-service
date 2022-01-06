using System.Collections.Generic;
using Dapper;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Data;

namespace SFA.DAS.ApplyService.Data.DapperTypeHandlers
{
    public class FinancialEvidencesDataHandler : SqlMapper.TypeHandler<List<FinancialEvidence>>
    {
        public override void SetValue(IDbDataParameter parameter, List<FinancialEvidence> value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }
    
        public override List<FinancialEvidence> Parse(object value)
        {
            return value == null ? null : JsonConvert.DeserializeObject<List<FinancialEvidence>>(value.ToString());
        }
    
    }

}
