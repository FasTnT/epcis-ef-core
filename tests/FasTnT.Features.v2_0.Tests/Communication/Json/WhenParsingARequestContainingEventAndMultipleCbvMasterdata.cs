﻿using FasTnT.Domain.Model;
using FasTnT.Features.v2_0.Communication.Json.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace FasTnT.Features.v2_0.Tests.Communication.Json;

[TestClass]
public class WhenParsingARequestContainingEventAndMultipleCbvMasterdata : JsonParsingTestCase
{
    public static readonly string ResourceName = "FasTnT.Features.v2_0.Tests.Resources.Requests.Request_ObjectEvent_WithMultipleMasterdata.json";

    public Request Request { get; set; }

    [TestInitialize]
    public void When()
    {
        Request = JsonEpcisDocumentParser.Parse(ParseResource(ResourceName), TestNamespaces.Default);
    }

    [TestMethod]
    public void RequestShouldContainsOneEvent()
    {
        Assert.AreEqual(1, Request.Events.Count);
    }

    [TestMethod]
    public void RequestShouldContainsAllTheMasterdata()
    {
        Assert.AreEqual(3, Request.Masterdata.Count);
        Assert.AreEqual(2, Request.Masterdata.Count(x => x.Type == "urn:epcglobal:epcis:vtype:ReadPoint"));
        Assert.AreEqual(1, Request.Masterdata.Count(x => x.Type == "urn:epcglobal:epcis:vtype:BizLocation"));
        Assert.AreEqual(2, Request.Masterdata.Single(x => x.Type == "urn:epcglobal:epcis:vtype:BizLocation").Attributes.Count);
        Assert.AreEqual(2, Request.Masterdata.Single(x => x.Type == "urn:epcglobal:epcis:vtype:BizLocation").Children.Count);
    }

    [TestMethod]
    public void RequestDateShouldBePopulated()
    {
        var expectedDate = new DateTimeOffset(2013, 06, 04, 12, 59, 02, 99, TimeSpan.Zero);
        Assert.AreEqual(expectedDate, Request.DocumentTime);
    }
}
