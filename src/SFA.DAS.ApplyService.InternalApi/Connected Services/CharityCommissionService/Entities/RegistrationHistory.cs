using System;
using System.Globalization;
using System.Xml.Serialization;

namespace CharityCommissionService
{
    public partial class RegistrationHistory
    {
        private readonly string[] expectedDateFormats = new string[] { @"dd/MM/yyyy", @"dd/MM/yyyy HH:mm:ss" };

        [XmlIgnore]
        public DateTime? RegistrationDateTime
        {
            get
            {
                return DateTime.TryParseExact(registrationDateField, expectedDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : default(DateTime?);
            }
        }

        [XmlIgnore]
        public DateTime? RegistrationRemovalDateTime
        {
            get
            {
                return DateTime.TryParseExact(registrationRemovalDateField, expectedDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : default(DateTime?);
            }
        }
    }
}
