using System;
using System.Globalization;
using System.Xml.Serialization;

namespace CharityCommissionService
{
    public partial class LatestFiling
    {
        private readonly string[] expectedDateFormats = new string[] { @"dd/MM/yyyy", @"dd/MM/yyyy HH:mm:ss" };

        [XmlIgnore]
        public DateTime? AnnualReturnPeriodDateTime
        {
            get
            {
                return DateTime.TryParseExact(annualReturnPeriodField, expectedDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : default(DateTime?);
            }
        }

        [XmlIgnore]
        public DateTime? AccountsPeriodDateTime
        {
            get
            {
                return DateTime.TryParseExact(accountsPeriodField, expectedDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : default(DateTime?);
            }
        }
    }
}
