﻿using FasTnT.Application.Database;
using FasTnT.Application.Services.Users;
using FasTnT.Application.Tests.Context;
using FasTnT.Application.Handlers;
using FasTnT.Domain.Enumerations;
using FasTnT.Domain.Model;
using FasTnT.Domain.Model.Events;

namespace FasTnT.Application.Tests.Capture;

[TestClass]
public class WhenHandlingCaptureRequest
{
    readonly static EpcisContext Context = EpcisTestContext.GetContext(nameof(WhenHandlingCaptureRequest));
    readonly static ICurrentUser UserContext = new TestCurrentUser();
    readonly static TestSubscriptionListener SubscriptionListener = new();

    [TestMethod]
    public void ItShouldReturnACaptureResultAndStoreTheRequest()
    {
        var handler = new CaptureHandler(Context, UserContext, SubscriptionListener);
        var request = new Request { SchemaVersion = "1.0", Events = new() { new Event { Type = EventType.ObjectEvent } } };
        var result = handler.StoreAsync(request, default).Result;

        Assert.IsNotNull(result);
        Assert.AreEqual(1, Context.Set<Request>().Count());
        Assert.IsTrue(SubscriptionListener.IsTriggered("stream"));
    }
}
