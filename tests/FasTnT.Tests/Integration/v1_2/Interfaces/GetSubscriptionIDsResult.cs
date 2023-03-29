﻿using System.Xml.Serialization;

namespace FasTnT.Tests.Integration.v1_2.Interfaces;

[XmlRoot("GetSubscriptionIDsResult", Namespace = "urn:epcglobal:epcis-query:xsd:1")]
public class GetSubscriptionIDsResult
{
    [XmlElement("string")]
    public string[] Values { get; set; }
}
