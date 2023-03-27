﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using FasTnT.Application.Services.Users;
using System.Text.Json;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace FasTnT.Host.Services.User;

public class BasicAuthentication : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string Authorization = nameof(Authorization);
    private const string Basic = nameof(Basic);

    public static string SchemeName => Basic + Authorization;

    public BasicAuthentication(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        return Context.GetEndpoint()?.Metadata?.GetMetadata<AuthorizeAttribute>() is null
            ? Task.Run(AuthenticateResult.NoResult)
            : Task.Run(ProcessAuthenticationAsync);
    }

    private AuthenticateResult ProcessAuthenticationAsync()
    {
        try
        {
            var authValue = RetrieveAuthorizationValue(Request);

            if (string.IsNullOrEmpty(authValue))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }
            else
            {
                var (username, password) = ParseAuthenticationHeader(authValue);

                return Authenticated(username, password, new[] { "fastnt.query", "fastnt.capture" });
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Invalid authorization information");

            return AuthenticateResult.Fail(ex.Message);
        }
    }

    private static string RetrieveAuthorizationValue(HttpRequest request)
    {
        if (request.Headers.TryGetValue(Authorization, out var headerValue))
        {
            var authHeader = AuthenticationHeaderValue.Parse(headerValue);

            if (authHeader.Scheme != Basic)
            {
                throw new Exception($"Invalid Authorization scheme: {authHeader.Scheme}");
            }

            return authHeader.Parameter;
        }
        else if (request.Query.TryGetValue("auth", out var queryValue))
        {
            return queryValue;
        }

        return null;
    }

    private static (string username, string password) ParseAuthenticationHeader(string authHeader)
    {
        var credentialBytes = Convert.FromBase64String(authHeader);
        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

        return credentials.Length == 2
            ? (credentials[0], credentials[1])
            : throw new FormatException("Authorization header must contain 2 values separated by ':'");
    }

    private static AuthenticateResult Authenticated(string username, string password, IEnumerable<string> requiredClaims)
    {
        var userHash = Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(string.Join('#', username, password))));
        var claims = new List<Claim>
        {
            new Claim(nameof(ICurrentUser.UserName), username),
            new Claim(nameof(ICurrentUser.UserId), userHash),
            new Claim(nameof(ICurrentUser.DefaultQueryParameters), JsonSerializer.Serialize(new[]{ new { Name = "EQ_userID", Values = new[] { userHash } } }))
        };
        claims.AddRange(requiredClaims.Select(x => new Claim(x, x)));

        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(claims, Basic)), Basic));
    }
}
