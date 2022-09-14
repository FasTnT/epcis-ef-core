﻿using FasTnT.Application.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using FasTnT.Features.v1_2.Endpoints.Interfaces;
using FasTnT.Application.Services.Capture;

namespace FasTnT.Features.v1_2.Endpoints;

public class CaptureEndpoints
{
    protected CaptureEndpoints() { }

    public static IEndpointRouteBuilder AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("v1_2/capture", HandleCaptureRequest).RequireAuthorization(policyNames: nameof(ICurrentUser.CanCapture));

        return app;
    }

    private static async Task<IResult> HandleCaptureRequest(CaptureRequest request, IStoreEpcisDocumentHandler handler, CancellationToken cancellationToken)
    {
        await handler.StoreAsync(request.Request, cancellationToken);

        return Results.NoContent();
    }
}
