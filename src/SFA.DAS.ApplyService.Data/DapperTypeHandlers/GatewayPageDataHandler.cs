using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Data.DapperTypeHandlers
{
    public class GatewayPageDataHandler : SqlMapper.TypeHandler<GatewayPageData>
    {
        public override GatewayPageData Parse(object value)
        {
            return JsonConvert.DeserializeObject<GatewayPageData>(value.ToString());
        }

        public override void SetValue(IDbDataParameter parameter, GatewayPageData value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }
    }
}
