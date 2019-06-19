using SFA.DAS.ApplyService.InternalApi.Models.Ukrlp;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public class UkrlpSoapSerializer : IUkrlpSoapSerializer
    {
        public string BuildUkrlpSoapRequest(long ukprn, string stakeholderId, string queryId)
        {
            var selectionCriteriaElement = new XElement("SelectionCriteria",
                new XElement("UnitedKingdomProviderReferenceNumberList",
                    new XElement("UnitedKingdomProviderReferenceNumber", new XText(ukprn.ToString()))),
                new XElement("CriteriaCondition", new XText("OR")),
                new XElement("StakeholderId", stakeholderId),
                new XElement("ApprovedProvidersOnly", "No"),
                new XElement("ProviderStatus", "A")
            );

            var queryIdElement = new XElement(XName.Get("QueryId"), new XText(queryId));

            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace ukr = "http://ukrlp.co.uk.server.ws.v3";

            var providerQueryRequest = new XElement(ukr + "ProviderQueryRequest", selectionCriteriaElement, queryIdElement);

            var soapBodyElement = new XElement(soapenv + "Body", providerQueryRequest);

            var soapHeaderElement = new XElement(soapenv + "Header");

            var soapEnvelope = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", soapenv.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "ukr", ukr.NamespaceName),
                soapHeaderElement, soapBodyElement);

            return soapEnvelope.ToString();
        }

        public MatchingProviderRecords DeserialiseMatchingProviderRecordsResponse(string soapXml)
        {
            var soapDocument = XDocument.Parse(soapXml);
            var queryResponse = soapDocument.XPathSelectElement("//MatchingProviderRecords");

            if (queryResponse == null)
            {
                return null;
            }

            // UKRLP SOAP service doesn't return contacts and verification details arrays in a 
            // wrapping tag, so can't serialize using XmlArray
            var matchingRecordsSerializer = new XmlSerializer(typeof(MatchingProviderRecords));
            var contactSerializer = new XmlSerializer(typeof(ProviderContactStructure));
            var verificationDetailsSerializer = new XmlSerializer(typeof(VerificationDetailsStructure));

            MatchingProviderRecords matchingProviderRecords =
                (MatchingProviderRecords)matchingRecordsSerializer.Deserialize(queryResponse.CreateReader());

            var contactElements = queryResponse.Descendants(XName.Get("ProviderContact"));
            if (contactElements != null)
            {
                matchingProviderRecords.ProviderContacts = new List<ProviderContactStructure>();
                foreach (var contactElement in contactElements)
                {
                    var contact =
                        (ProviderContactStructure)contactSerializer.Deserialize(contactElement.CreateReader());
                    matchingProviderRecords.ProviderContacts.Add(contact);
                }
            }

            var verificationElements = queryResponse.Descendants(XName.Get("VerificationDetails"));
            if (verificationElements != null)
            {
                matchingProviderRecords.VerificationDetails = new List<VerificationDetailsStructure>();
                foreach (var verificationElement in verificationElements)
                {
                    var verification =
                        (VerificationDetailsStructure)verificationDetailsSerializer.Deserialize(verificationElement
                            .CreateReader());
                    matchingProviderRecords.VerificationDetails.Add(verification);
                }
            }

            return matchingProviderRecords;
        }
    }
}
