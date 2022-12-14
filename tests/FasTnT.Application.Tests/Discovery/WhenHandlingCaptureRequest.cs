﻿using FasTnT.Application.Database;
using FasTnT.Application.Services.Users;
using FasTnT.Application.Tests.Context;
using FasTnT.Application.UseCases.TopLevelResources;
using FasTnT.Domain.Model.Events;
using FasTnT.Domain.Model.Queries;

namespace FasTnT.Application.Tests.Discovery;

[TestClass]
public class WhenHandlingListEpcsRequest
{
    readonly static EpcisContext Context = EpcisTestContext.GetContext(nameof(WhenHandlingListEpcsRequest));
    readonly static ICurrentUser UserContext = new TestCurrentUser();

    [TestInitialize]
    public void Initialize()
    {
        Context.Add(new Domain.Model.Request
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
                        new Epc { Type = Domain.Enumerations.EpcType.List, Id = "test:epc:1" },
                        new Epc { Type = Domain.Enumerations.EpcType.List, Id = "test:epc:2" },
                        new Epc { Type = Domain.Enumerations.EpcType.List, Id = "test:epc:3" }
                    }
                }
            }
        });

        Context.SaveChanges();
    }

    [TestMethod]
    public void ItShouldReturnAllTheEpcsIfPageSizeIsGreaterThanNumberOfEpcs()
    {
        var handler = new TopLevelResourceUseCasesHandler(Context, UserContext);
        var request = new Pagination(10, 0);

        var result = handler.ListEpcs(request, default).Result;

        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count());
    }

    [TestMethod]
    public void ItShouldReturnTheRequestedNumberOfEpcsIfPageSizeIsLowerThanNumberOfEpcs()
    {
        var handler = new TopLevelResourceUseCasesHandler(Context, UserContext);
        var request = new Pagination(1, 0);

        var result = handler.ListEpcs(request, default).Result;

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void ItShouldReturnTheCorrectPageOfData()
    {
        var handler = new TopLevelResourceUseCasesHandler(Context, UserContext);
        var request = new Pagination(3, 2);

        var result = handler.ListEpcs(request, default).Result;

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
    }
}
