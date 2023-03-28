﻿using FasTnT.Host.Features.v2_0.Endpoints;

namespace FasTnT.Host.Features.v2_0;

public static class Epcis2_0Configuration
{
    public static WebApplication UseEpcis20Endpoints(this WebApplication app)
    {
        app.UseWebSockets();

        CaptureEndpoints.AddRoutes(app);
        EventsEndpoints.AddRoutes(app);
        QueriesEndpoints.AddRoutes(app);
        TopLevelEndpoints.AddRoutes(app);
        SubscriptionEndpoints.AddRoutes(app);
        DiscoveryEndpoints.AddRoutes(app);

        return app;
    }

    internal static RouteHandlerBuilder Get(this IEndpointRouteBuilder endpoints, string pattern, Delegate handler)
    {
        return MapMethod(endpoints, HttpMethods.Get, pattern, handler);
    }

    internal static RouteHandlerBuilder Delete(this IEndpointRouteBuilder endpoints, string pattern, Delegate handler)
    {
        return MapMethod(endpoints, HttpMethods.Delete, pattern, handler);
    }

    internal static RouteHandlerBuilder Post(this IEndpointRouteBuilder endpoints, string pattern, Delegate handler)
    {
        return MapMethod(endpoints, HttpMethods.Post, pattern, handler);
    }

    internal static RouteHandlerBuilder Options(this IEndpointRouteBuilder endpoints, string pattern, Delegate handler)
    {
        return MapMethod(endpoints, HttpMethods.Options, pattern, handler);
    }

    private static RouteHandlerBuilder MapMethod(IEndpointRouteBuilder endpoints, string method, string pattern, Delegate handler)
    {
        if (method != HttpMethods.Options)
        {
            DiscoveryEndpoints.Endpoints.Add((pattern, method));
        }

        return endpoints.MapMethods(pattern, new[] { method }, DelegateFactory.Create(_ => handler));
    }
}
