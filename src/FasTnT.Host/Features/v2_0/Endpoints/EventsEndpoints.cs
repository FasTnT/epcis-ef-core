﻿using FasTnT.Application.Domain.Model.Queries;
using FasTnT.Application.Handlers;
using FasTnT.Host.Features.v2_0.Endpoints.Interfaces;
using FasTnT.Host.Features.v2_0.Endpoints.Interfaces.Utils;

namespace FasTnT.Host.Features.v2_0.Endpoints;

public static class EventsEndpoints
{
    public static void AddRoutes(IEndpointRouteBuilder app)
    {
        app.Get("v2_0/events", EventQuery).RequireAuthorization("query");
        app.Get("v2_0/events/{*eventId}", SingleEventQuery).RequireAuthorization("query");
        app.Get("v2_0/eventTypes/{eventType}/events", EventTypeQuery).RequireAuthorization("query");
        app.Get("v2_0/epcs/{epc}/events", EpcQuery).RequireAuthorization("query");
        app.Get("v2_0/bizSteps/{bizStep}/events", BizStepQuery).RequireAuthorization("query");
        app.Get("v2_0/bizLocations/{bizLocation}/events", BizLocationQuery).RequireAuthorization("query");
        app.Get("v2_0/readPoints/{readPoint}/events", ReadPointQuery).RequireAuthorization("query");
        app.Get("v2_0/dispositions/{disposition}/events", DispositionQuery).RequireAuthorization("query");
    }

    private static Task<IResult> EventQuery(QueryContext parameters, DataRetrieverHandler handler, CancellationToken cancellationToken)
    {
        return ExecuteQuery(handler, parameters.Parameters, cancellationToken);
    }

    private static Task<IResult> SingleEventQuery(string eventId, DataRetrieverHandler handler, CancellationToken cancellationToken)
    {
        var parameter = QueryParameter.Create("EQ_eventID", eventId);

        return ExecuteQuery(handler, new[] { parameter }, cancellationToken);
    }

    private static Task<IResult> EventTypeQuery(string eventType, QueryContext context, DataRetrieverHandler handler, CancellationToken cancellationToken)
    {
        var parameters = context.Parameters.Append(QueryParameter.Create("eventType", eventType));

        return ExecuteQuery(handler, parameters, cancellationToken);
    }

    private static Task<IResult> EpcQuery(string epc, QueryContext queryParams, DataRetrieverHandler handler, CancellationToken cancellationToken)
    {
        var parameters = queryParams.Parameters.Append(QueryParameter.Create("MATCH_anyEPC", epc));

        return ExecuteQuery(handler, parameters, cancellationToken);
    }

    private static Task<IResult> BizStepQuery(string bizStep, QueryContext context, DataRetrieverHandler handler, CancellationToken cancellationToken)
    {
        var parameters = context.Parameters.Append(QueryParameter.Create("EQ_bizStep", bizStep));

        return ExecuteQuery(handler, parameters, cancellationToken);
    }

    private static Task<IResult> BizLocationQuery(string bizLocation, QueryContext context, DataRetrieverHandler handler, CancellationToken cancellationToken)
    {
        var parameters = context.Parameters.Append(QueryParameter.Create("EQ_bizLocation", bizLocation));

        return ExecuteQuery(handler, parameters, cancellationToken);
    }

    private static Task<IResult> ReadPointQuery(string readPoint, QueryContext context, DataRetrieverHandler handler, CancellationToken cancellationToken)
    {
        var parameters = context.Parameters.Append(QueryParameter.Create("EQ_readPoint", readPoint));

        return ExecuteQuery(handler, parameters, cancellationToken);
    }

    private static Task<IResult> DispositionQuery(string disposition, QueryContext context, DataRetrieverHandler handler, CancellationToken cancellationToken)
    {
        var parameters = context.Parameters.Append(QueryParameter.Create("EQ_disposition", disposition));

        return ExecuteQuery(handler, parameters, cancellationToken);
    }

    private static async Task<IResult> ExecuteQuery(DataRetrieverHandler handler, IEnumerable<QueryParameter> parameters, CancellationToken cancellationToken)
    {
        var response = await handler.QueryEventsAsync(parameters, cancellationToken);

        return EpcisResults.Ok(new QueryResult(new ("SimpleEventQuery", response)));
    }
}
