using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Data.DapperTypeHandlers
{
    public class ApplicationDataHandler : SqlMapper.TypeHandler<ApplicationData>
    {
        public override void SetValue(IDbDataParameter parameter, ApplicationData value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }

        public override ApplicationData Parse(object value)
        {
            return JsonConvert.DeserializeObject<ApplicationData>(value.ToString());
        }
        
    }
}
