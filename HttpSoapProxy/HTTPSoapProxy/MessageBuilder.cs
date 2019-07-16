using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace HTTPSoapProxy
{
    public class MessageBuilder
    {

        public static XDocument BuildMessage(int value1, int value2)
        {
            XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace myns = "http://tempuri.org/";

            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace xsd = "http://www.w3.org/2001/XMLSchema";

            XDocument soapRequest = new XDocument(
                new XDeclaration("1.0", "UTF-8", "no"),
                new XElement(ns + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                    new XAttribute(XNamespace.Xmlns + "xsd", xsd),
                    new XAttribute(XNamespace.Xmlns + "soap", ns),
                    new XElement(ns + "Body",
                        new XElement(myns + "Add",new XElement(myns + "intA", value1), new XElement(myns + "intB", value2)))
                ));

            return soapRequest;
        }
    }
}
