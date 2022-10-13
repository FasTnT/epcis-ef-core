﻿using FasTnT.Application.UseCases.TopLevelResources;
using FasTnT.Features.v2_0.Endpoints.Interfaces;
using FasTnT.Features.v2_0.Endpoints.Interfaces.Utils;

namespace FasTnT.Features.v2_0.Endpoints;

public static class TopLevelEndpoints
{
    public static IEndpointRouteBuilder AddRoutes(IEndpointRouteBuilder app)
    {
        app.TryMapGet("v2_0/eventTypes", HandleListEventTypes).RequireAuthorization("query");
        app.TryMapGet("v2_0/epcs", HandleListEpcs).RequireAuthorization("query");
        app.TryMapGet("v2_0/bizSteps", HandleListBizSteps).RequireAuthorization("query");
        app.TryMapGet("v2_0/bizLocations", HandleListBizLocations).RequireAuthorization("query");
        app.TryMapGet("v2_0/readPoints", HandleListReadPoints).RequireAuthorization("query");
        app.TryMapGet("v2_0/dispositions", HandleListDispositions).RequireAuthorization("query");
        app.TryMapGet("v2_0/eventTypes/{eventType}", HandleSubResourceRequest).RequireAuthorization("query");
        app.TryMapGet("v2_0/epcs/{epc}", HandleSubResourceRequest).RequireAuthorization("query");
        app.TryMapGet("v2_0/bizSteps/{bizStep}", HandleSubResourceRequest).RequireAuthorization("query");
        app.TryMapGet("v2_0/bizLocations/{bizLocation}", HandleSubResourceRequest).RequireAuthorization("query");
        app.TryMapGet("v2_0/readPoints/{readPoint}", HandleSubResourceRequest).RequireAuthorization("query");
        app.TryMapGet("v2_0/dispositions/{disposition}", HandleSubResourceRequest).RequireAuthorization("query");

        return app;
    }

    private static async Task<IResult> HandleListEventTypes(IListEventTypesHandler handler, CancellationToken cancellationToken)
    {
        var response = await handler.ListEventTypes(cancellationToken);

        return EpcisResults.Ok(new CollectionResult(response));
    }

    private static async Task<IResult> HandleListEpcs(IListEpcsHandler handler, CancellationToken cancellationToken)
    {
        var response = await handler.ListEpcs(cancellationToken);

        return EpcisResults.Ok(new CollectionResult(response));
    }

    private static async Task<IResult> HandleListBizSteps(IListBizStepsHandler handler, CancellationToken cancellationToken)
    {
        var response = await handler.ListBizSteps(cancellationToken);

        return EpcisResults.Ok(new CollectionResult(response));
    }

    private static async Task<IResult> HandleListBizLocations(IListBizLocationsHandler handler, CancellationToken cancellationToken)
    {
        var response = await handler.ListBizLocations(cancellationToken);

        return EpcisResults.Ok(new CollectionResult(response));
    }

    private static async Task<IResult> HandleListReadPoints(IListReadPointsHandler handler, CancellationToken cancellationToken)
    {
        var response = await handler.ListReadPoints(cancellationToken);

        return EpcisResults.Ok(new CollectionResult(response));
    }

    private static async Task<IResult> HandleListDispositions(IListDispositionsHandler handler, CancellationToken cancellationToken)
    {
        var response = await handler.ListDispositions(cancellationToken);

        return EpcisResults.Ok(new CollectionResult(response));
    }

    private static IResult HandleSubResourceRequest()
    {
        return EpcisResults.Ok(new CollectionResult(new[] { "events" }));
    }
}
