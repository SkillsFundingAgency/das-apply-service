using System;
using System.Xml;
using System.Xml.Serialization;

namespace CharityCommissionService
{
    public partial class LatestFiling
    {
        [XmlIgnore]
        public DateTime? AnnualReturnPeriodDateTime
        {
            get
            {
                try
                {
                    return XmlConvert.ToDateTime(this.annualReturnPeriodField, @"dd/MM/yyyy");
                }
                catch
                {
                    return null;
                }
            }
        }

        [XmlIgnore]
        public DateTime? AccountsPeriodDateTime
        {
            get
            {
                try
                {
                    return XmlConvert.ToDateTime(this.accountsPeriodField, @"dd/MM/yyyy");
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
