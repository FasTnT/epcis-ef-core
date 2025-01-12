﻿using FasTnT.Domain.Exceptions;
using FasTnT.Host.Communication.Xml.Formatters;
using System.Xml.Linq;

namespace FasTnT.Host.Tests.Features.v1_2.Communication;

[TestClass]
public class WhenFormattingAnUnknownResponse
{
    public XElement Formatted { get; set; }

    [TestInitialize]
    public void When()
    {
        Formatted = SoapResponseFormatter.Format(new { Type = "Unknown object" });
    }

    [TestMethod]
    public void ItShouldReturnAnXElement()
    {
        Assert.IsNotNull(Formatted);
    }

    [TestMethod]
    public void TheXmlShouldBeAnImplementationException()
    {
        Assert.IsTrue(Formatted.Name == XName.Get("ImplementationException", "urn:epcglobal:epcis-query:xsd:1"));
    }
}

[TestClass]
public class ErrorResponseStatusCodesTests
{
    [TestMethod]
    public void TheStatusCodeShouldBeCorrectForErrorResults()
    {
        Assert.AreEqual(404, XmlResponseFormatter.GetHttpStatusCode(new EpcisException(ExceptionType.NoSuchNameException, null)));
        Assert.AreEqual(404, XmlResponseFormatter.GetHttpStatusCode(new EpcisException(ExceptionType.NoSuchSubscriptionException, null)));
        Assert.AreEqual(413, XmlResponseFormatter.GetHttpStatusCode(new EpcisException(ExceptionType.QueryTooComplexException, null)));
        Assert.AreEqual(500, XmlResponseFormatter.GetHttpStatusCode(new EpcisException(ExceptionType.ImplementationException, null)));
        Assert.AreEqual(400, XmlResponseFormatter.GetHttpStatusCode(new EpcisException(ExceptionType.ValidationException, null)));
        Assert.AreEqual(413, XmlResponseFormatter.GetHttpStatusCode(new EpcisException(ExceptionType.CaptureLimitExceededException, null)));
        Assert.AreEqual(400, XmlResponseFormatter.GetHttpStatusCode(new EpcisException(ExceptionType.DuplicateSubscriptionException, null)));
        Assert.AreEqual(400, XmlResponseFormatter.GetHttpStatusCode(new EpcisException(ExceptionType.InvalidURIException, null)));
        Assert.AreEqual(413, XmlResponseFormatter.GetHttpStatusCode(new EpcisException(ExceptionType.QueryTooLargeException, null)));
        Assert.AreEqual(400, XmlResponseFormatter.GetHttpStatusCode(new EpcisException(ExceptionType.SubscriptionControlsException, null)));
    }
}
