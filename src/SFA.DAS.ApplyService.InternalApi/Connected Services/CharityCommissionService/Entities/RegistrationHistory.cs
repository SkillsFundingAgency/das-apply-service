using System;
using System.Xml;
using System.Xml.Serialization;

namespace CharityCommissionService
{
    public partial class RegistrationHistory
    {
        [XmlIgnore]
        public DateTime? RegistrationDateTime
        {
            get
            {
                try
                {
                    return XmlConvert.ToDateTime(this.registrationDateField, new string[] { @"dd/MM/yyyy", @"dd/MM/yyyy HH:mm:ss"});
                }
                catch
                {
                    return null;
                }
            }
        }

        [XmlIgnore]
        public DateTime? RegistrationRemovalDateTime
        {
            get
            {
                try
                {
                    return XmlConvert.ToDateTime(this.registrationRemovalDateField, new string[] { @"dd/MM/yyyy", @"dd/MM/yyyy HH:mm:ss" });
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
