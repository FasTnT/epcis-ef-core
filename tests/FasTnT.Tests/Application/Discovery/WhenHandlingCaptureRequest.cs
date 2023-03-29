﻿using FasTnT.Application.Handlers;
using FasTnT.Application.Domain.Model.Events;
using FasTnT.Tests.Application.Context;
using FasTnT.Application.Domain.Enumerations;

namespace FasTnT.Tests.Application.Discovery;

[TestClass]
public class WhenHandlingListEpcsRequest
{
    readonly static EpcisContext Context = EpcisTestContext.GetContext(nameof(WhenHandlingListEpcsRequest));
    readonly static ICurrentUser UserContext = new TestCurrentUser();

    [ClassCleanup]
    public static void Cleanup()
    {
        if (Context != null)
        {
            Context.Database.EnsureDeleted();
        }
    }

    [TestInitialize]
    public void Initialize()
    {
        Context.Add(new Request
        {
            CaptureTime = DateTime.Now,
            DocumentTime = DateTime.Now,
            SchemaVersion = "2.0",
            UserId = "TESTUSER",
            Events = new List<Event>
            {
                new Event
                {
                    Epcs = new List<Epc> {
                        new Epc { Type = EpcType.List, Id = "test:epc:1" },
                        new Epc { Type = EpcType.List, Id = "test:epc:2" },
                        new Epc { Type = EpcType.List, Id = "test:epc:3" }
                    }
                }
            }
        });

        Context.SaveChanges();
    }

    [TestMethod]
    public void ItShouldReturnAllTheEpcsIfPageSizeIsGreaterThanNumberOfEpcs()
    {
        var handler = new TopLevelResourceHandler(Context, UserContext);
        var request = new Pagination(10, 0);

        var result = handler.ListEpcs(request, default).Result;

        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count());
    }

    [TestMethod]
    public void ItShouldReturnTheRequestedNumberOfEpcsIfPageSizeIsLowerThanNumberOfEpcs()
    {
        var handler = new TopLevelResourceHandler(Context, UserContext);
        var request = new Pagination(1, 0);

        var result = handler.ListEpcs(request, default).Result;

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void ItShouldReturnTheCorrectPageOfData()
    {
        var handler = new TopLevelResourceHandler(Context, UserContext);
        var request = new Pagination(3, 2);

        var result = handler.ListEpcs(request, default).Result;

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
    }
}
