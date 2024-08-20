﻿using FasTnT.Application.Services.Users;
using FasTnT.Domain.Model.Queries;
using System.Text.Json;

namespace FasTnT.Host.Services.User;

public class HttpContextCurrentUser : ICurrentUser
{
    public string UserName { get; init; }
    public string UserId { get; init; }
    public IEnumerable<QueryParameter> DefaultQueryParameters { get; init; }

    public HttpContextCurrentUser(IHttpContextAccessor contextAccessor)
    {
        var user = contextAccessor?.HttpContext?.User;

        if (user == default)
        {
            return;
        }

        var parameters = user.FindFirst(x => x.Type == nameof(DefaultQueryParameters))?.Value ?? "[]";

        UserName = user.Claims.Single(x => x.Type == nameof(UserName)).Value;
        UserId = user.Claims.Single(x => x.Type == nameof(UserId)).Value;
        DefaultQueryParameters = JsonSerializer.Deserialize<IEnumerable<QueryParameter>>(parameters);
    }
}
