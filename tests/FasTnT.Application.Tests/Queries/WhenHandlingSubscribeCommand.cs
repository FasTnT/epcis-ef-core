﻿using FasTnT.Application.Database;
using FasTnT.Application.Tests.Subscriptions;
using FasTnT.Application.UseCases.Subscriptions;
using FasTnT.Domain.Exceptions;
using FasTnT.Domain.Model.Subscriptions;

namespace FasTnT.Application.Tests.Queries;

[TestClass]
public class WhenHandlingSubscribeCommand
{
    readonly static EpcisContext Context = Tests.Context.EpcisTestContext.GetContext(nameof(WhenHandlingSubscribeCommand));

    [ClassInitialize]
    public static void Initialize(TestContext _)
    {
        Context.Add(new Subscription
        {
            Name = "TestSubscription",
            QueryName = "SimpleEventQuery",
            Destination = "",
            FormatterName = "TestFormatter"
        });

        Context.SaveChanges();
    }

    [TestMethod]
    public void ItShouldThrowAnExceptionIfASubscriptionWithTheSameNameAlreadyExist()
    {
        var subscription = new Subscription
        {
            Name = "TestSubscription",
            Destination = "",
            QueryName = "SimpleEventQuery"
        };
        var handler = new SubscriptionsUseCasesHandler(Context, null);

        Assert.ThrowsExceptionAsync<EpcisException>(() => handler.RegisterSubscriptionAsync(subscription, new TestResultSender(), CancellationToken.None));
    }

    [TestMethod]
    public void ItShouldThrowAnExceptionIfTheSpecifiedQueryNameDoesNotExist()
    {
        var subscription = new Subscription
        {
            Name = "InvalidSubscription",
            Destination = "",
            QueryName = "UnknownQuery"
        };
        var handler = new SubscriptionsUseCasesHandler(Context, null);

        Assert.ThrowsExceptionAsync<EpcisException>(() => handler.RegisterSubscriptionAsync(subscription, new TestResultSender(), CancellationToken.None));
    }

    [TestMethod]
    public void ItShouldThrowAnExceptionIfTheSpecifiedQueryDoesNotAllowSubscription()
    {
        var subscription = new Subscription
        {
            Name = "MasterdataTestSubscription",
            Destination = "",
            QueryName = "SimpleMasterdataQuery"
        };
        var handler = new SubscriptionsUseCasesHandler(Context, null);

        Assert.ThrowsExceptionAsync<EpcisException>(() => handler.RegisterSubscriptionAsync(subscription, new TestResultSender(), CancellationToken.None));
    }
}
