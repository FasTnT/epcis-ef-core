﻿using FasTnT.Application.Queries;
using FasTnT.Application.Services;
using FasTnT.Domain.Queries;
using FasTnT.Infrastructure.Store;

namespace FasTnT.Application.Tests.Queries
{
    [TestClass]
    public class WhenSimpleEventQueryReturnsLessThanMaxEventCountParameter
    {
        public EpcisContext Context { get; set; }
        public IEpcisQuery Query { get; set; }
        public IList<QueryParameter> Parameters { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Context = Tests.Context.TestContext.GetContext("simpleEventQuery");
            Query = new SimpleEventQuery(Context);

            Context.Requests.Add(new Domain.Model.Request
            {
                Events = new[] {
                    new Domain.Model.Event
                    {
                        Action = Domain.Enumerations.EventAction.Observe
                    }
                }.ToList(),
                CaptureDate = DateTime.Now,
                DocumentTime = DateTime.Now,
                SchemaVersion = "1.2"
            });
            Context.SaveChanges();

            Parameters = new[] { new QueryParameter("maxEventCount", new[] { "2" }) }.ToList();
        }

        [TestMethod]
        public void ItShouldThrowAQueryTooLargeExceptionException()
        {
            var result = Query.HandleAsync(Parameters, default).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.EventList.Count, 1);
        }
    }
}
