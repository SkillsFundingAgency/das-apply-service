using System.Collections.Generic;
using Dapper;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Data;

namespace SFA.DAS.ApplyService.Data.DapperTypeHandlers
{
    //MFCMFC remove
    // public class FinancialReviewDetailsDataHandler : SqlMapper.TypeHandler<FinancialReviewDetails>
    // {
    //     public override void SetValue(IDbDataParameter parameter, FinancialReviewDetails value)
    //     {
    //         parameter.Value = JsonConvert.SerializeObject(value);
    //     }
    //
    //     public override FinancialReviewDetails Parse(object value)
    //     {
    //         return JsonConvert.DeserializeObject<FinancialReviewDetails>(value.ToString());
    //     }
    //
    // }
    public class FinancialEvidencesDataHandler : SqlMapper.TypeHandler<List<FinancialEvidence>>
    {
        public override void SetValue(IDbDataParameter parameter, List<FinancialEvidence> value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }
    
        public override List<FinancialEvidence> Parse(object value)
        {
            if (value == null)
                return null;
            return JsonConvert.DeserializeObject<List<FinancialEvidence>>(value.ToString());
        }
    
    }

}
