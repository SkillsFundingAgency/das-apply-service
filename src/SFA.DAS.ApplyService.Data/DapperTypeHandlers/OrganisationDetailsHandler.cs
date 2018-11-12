using System.Data;
using Dapper;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Data.DapperTypeHandlers
{
    public class OrganisationDetailsHandler : SqlMapper.TypeHandler<OrganisationDetails>
    {
        public override OrganisationDetails Parse(object value)
        {
            return JsonConvert.DeserializeObject<OrganisationDetails>(value.ToString());
        }

        public override void SetValue(IDbDataParameter parameter, OrganisationDetails value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }
    }
}
