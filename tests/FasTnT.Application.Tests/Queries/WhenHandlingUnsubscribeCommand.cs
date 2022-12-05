﻿using FasTnT.Application.EfCore.Services.Queries;
using FasTnT.Application.EfCore.UseCases.Subscriptions;
using FasTnT.Application.Services.Queries;
using FasTnT.Application.Services.Subscriptions;
using FasTnT.Domain.Infrastructure.Exceptions;
using FasTnT.Domain.Model.Subscriptions;
using FasTnT.EfCore.Store;
using Moq;

namespace FasTnT.Application.Tests.Queries;

[TestClass]
public class WhenHandlingUnsubscribeCommand
{
    readonly static EpcisContext Context = Tests.Context.EpcisTestContext.GetContext(nameof(WhenHandlingUnsubscribeCommand));
    readonly static IEnumerable<IEpcisDataSource> Queries = new IEpcisDataSource[] { new SimpleEventQuery(Context), new SimpleMasterDataQuery(Context) };
    readonly static Mock<ISubscriptionListener> Listener = new(MockBehavior.Loose);

    [ClassInitialize]
    public static void Initialize(TestContext _)
    {
        Context.Subscriptions.Add(new Subscription
        {
            Name = "TestSubscription",
            QueryName = Queries.First().Name,
            FormatterName = "TestFormatter"
        });

        Context.SaveChanges();
    }

    [TestMethod]
    public void ItShouldReturnAnUnubscribeResult()
    {
        var subscription = "TestSubscription";
        var handler = new SubscriptionsUseCasesHandler(Context, Queries, Listener.Object);
        handler.DeleteSubscriptionAsync(subscription, CancellationToken.None).Wait();

        Assert.AreEqual(0, Context.Subscriptions.Count());
        Listener.Verify(x => x.RemoveAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public void ItShouldThrowAnExceptionIfASubscriptionWithTheSameNameDoesNotExist()
    {
        var subscription = "UnknownSubscription";
        var handler = new SubscriptionsUseCasesHandler(Context, null, default);

        Assert.ThrowsExceptionAsync<EpcisException>(() => handler.DeleteSubscriptionAsync(subscription, CancellationToken.None));
    }
}
