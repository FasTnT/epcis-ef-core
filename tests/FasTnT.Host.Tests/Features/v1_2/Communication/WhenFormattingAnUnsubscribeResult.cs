﻿using FasTnT.Host.Features.v1_2.Communication.Formatters;
using System.Xml.Linq;

namespace FasTnT.Host.Tests.Features.v1_2.Communication;

[TestClass]
public class WhenFormattingAnUnsubscribeResult
{
    public XElement Formatted { get; set; }

    [TestInitialize]
    public void When()
    {
        Formatted = XmlResponseFormatter.FormatUnsubscribeResponse();
    }

    [TestMethod]
    public void ItShouldReturnAnXElement()
    {
        Assert.IsNotNull(Formatted);
    }

    [TestMethod]
    public void TheXmlShouldBeCorrectlyFormatter()
    {
        Assert.IsTrue(Formatted.Name == XName.Get("UnsubscribeResult", "urn:epcglobal:epcis-query:xsd:1"));
        Assert.IsTrue(Formatted.IsEmpty);
    }
}
