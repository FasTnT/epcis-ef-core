using FasTnT.Application.Domain.Enumerations;
using FasTnT.Application.Domain.Model.Events;
using FasTnT.Host.Features.v2_0.Communication.Parsers;

namespace FasTnT.Tests.Features.v2_0.Communication.Json;

[TestClass]
public class WhenParsingAValidObjectEvent : JsonParsingTestCase
{
    public static readonly string ResourceName = "FasTnT.Tests.Features.v2_0.Communication.Resources.Events.ObjectEvent_Full.json";

    public Event Event { get; set; }

    [TestInitialize]
    public void When()
    {
        var parser = JsonEventParser.Create(ParseResource(ResourceName).RootElement, TestNamespaces.Default);
        Event = parser.Parse();
    }

    [TestMethod]
    public void ActionShouldBeParsedCorrectly()
    {
        Assert.AreEqual(EventAction.Observe, Event.Action);
    }

    [TestMethod]
    public void EventTimeShouldBeParsedCorrectly()
    {
        var expectedDate = new DateTime(2021, 02, 15, 14, 00, 00);
        Assert.AreEqual(expectedDate, Event.EventTime);
    }

    [TestMethod]
    public void EventTimeZoneOffsetShouldBeParsedCorrectly()
    {
        Assert.IsNotNull(Event.EventTimeZoneOffset);
        Assert.AreEqual("-06:00", Event.EventTimeZoneOffset.Representation);
    }

    [TestMethod]
    public void BizStepShouldBeParsedCorrectly()
    {
        Assert.AreEqual("inspecting", Event.BusinessStep);
    }

    [TestMethod]
    public void DispositionShouldBeParsedCorrectly()
    {
        Assert.AreEqual("active", Event.Disposition);
    }

    [TestMethod]
    public void ReadPointShouldBeParsedCorrectly()
    {
        Assert.AreEqual("urn:epc:id:sgln:4012345.00005.0", Event.ReadPoint);
    }

    [TestMethod]
    public void BizLocationShouldBeParsedCorrectly()
    {
        Assert.AreEqual("urn:epc:id:sgln:409876.500001.0", Event.BusinessLocation);
    }

    [TestMethod]
    public void BizTransactionsShouldBeParsedCorrectly()
    {
        Assert.AreEqual(2, Event.Transactions.Count);
        Assert.IsTrue(Event.Transactions.Any(x => x.Type == "desadv" && x.Id == "urn:epcglobal:cbv:bt:8779891013658:H9022413"));
        Assert.IsTrue(Event.Transactions.Any(x => x.Type == "po" && x.Id == "urn:epcglobal:cbv:bt:8811891013778:PO654321"));
    }

    [TestMethod]
    public void EventShouldContainsAllEpcListAndQuantities()
    {
        Assert.AreEqual(3, Event.Epcs.Count);

        Assert.IsTrue(Event.Epcs.Any(e => e.Id == "urn:epc:id:sscc:4001356.5900000817"), "EPC urn:epc:id:sscc:4001356.5900000817 is expected");
        Assert.IsTrue(Event.Epcs.Any(e => e.Id == "urn:epc:id:sscc:4001356.5900000822"), "EPC urn:epc:id:sscc:4001356.5900000822 is expected");
        Assert.IsTrue(Event.Epcs.Any(e => e.Id == "urn:epc:class:lgtin:409876.0000001.L1" && e.Quantity == 3500 && e.UnitOfMeasure == "KGM"), "Quantity EPC urn:epc:class:lgtin:409876.0000001.L1 is expected");
    }
}
