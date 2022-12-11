﻿using FasTnT.Domain.Model.Events;
using FasTnT.Host.Features.v2_0.Communication.Json.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FasTnT.Host.Tests.Features.v2_0.Communication.Json;

[TestClass]
public class WhenParsingAnObjectEventWithStringCertificationInfo : JsonParsingTestCase
{
    public static readonly string ResourceName = "FasTnT.Host.Tests.Features.v2_0.Communication.Resources.Events.ObjectEvent_StringCertificationInfo.json";

    public Event Event { get; set; }

    [TestInitialize]
    public void When()
    {
        var parser = JsonEventParser.Create(ParseResource(ResourceName).RootElement, TestNamespaces.Default);
        Event = parser.Parse();
    }

    [TestMethod]
    public void CertificationInfoShouldBeParsedCorrectly()
    {
        Assert.AreEqual("urn:fastnt:cert:115120.021", Event.CertificationInfo);
    }
}
