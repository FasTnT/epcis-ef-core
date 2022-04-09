﻿using FasTnT.Application.Queries;
using FasTnT.Domain.Queries;
using FasTnT.Infrastructure.Store;

namespace FasTnT.Application.Tests;

[TestClass]
public class WhenHandlingGetSubscriptionIdsQuery
{
    public readonly static EpcisContext Context = Tests.Context.TestContext.GetContext(nameof(Tests.WhenHandlingGetSubscriptionIdsQuery));

    [ClassInitialize]
    public static void Initialize(TestContext _)
    {
        Context.Subscriptions.Add(new Domain.Model.Subscription
        {
            Name = "SubscriptionTest",
            QueryName = "TestQuery"
        });
        Context.Subscriptions.Add(new Domain.Model.Subscription
        {
            Name = "OtherSubscription",
            QueryName = "OtherQuery"
        });

        Context.SaveChanges();
    }

    [TestMethod]



    public void ItShouldReturnTheListOfExistingSubscriptionIdsForTheSpecifiedRequest()
    {
        var handler = new GetSubscriptionIdsQueryHandler(Context);
        var result = handler.Handle(new GetSubscriptionIdsQuery { QueryName = "TestQuery" }, default).Result;
            
        Assert.AreEqual(1, result.SubscriptionIDs.Count());
        Assert.AreEqual("SubscriptionTest", result.SubscriptionIDs.First());
    }

    [TestMethod]
    public void ItShouldReturnAnEmptyListWhenNoSubscriptionMatch()
    {
        var handler = new GetSubscriptionIdsQueryHandler(Context);
        var result = handler.Handle(new GetSubscriptionIdsQuery { QueryName = "UnknownQuery" }, default).Result;
            
        Assert.AreEqual(0, result.SubscriptionIDs.Count());
    }
}
