using Dapper;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Data;

namespace SFA.DAS.ApplyService.Data.DapperTypeHandlers
{
    public class FinancialApplicationGradeDataHandler : SqlMapper.TypeHandler<FinancialApplicationGrade>
    {
        public override void SetValue(IDbDataParameter parameter, FinancialApplicationGrade value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }

        public override FinancialApplicationGrade Parse(object value)
        {
            return JsonConvert.DeserializeObject<FinancialApplicationGrade>(value.ToString());
        }

    }
}
