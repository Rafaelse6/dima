﻿using Dima.Api.Common.Api;
using Dima.Api.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Identity
{
    public class GetRolesEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
           app.MapGet("/roles", Handle).RequireAuthorization();

        private static Task<IResult> Handle(ClaimsPrincipal user)
        {

            if (user.Identity is null || !user.Identity.IsAuthenticated)
                return Task.FromResult<IResult>(Results.Unauthorized());
            var identity = (ClaimsIdentity)user.Identity;
            var roles = identity
               .FindAll(identity.RoleClaimType)
               .Select(c => new
               {
                   c.Issuer,
                   c.OriginalIssuer,
                   c.Type,
                   c.Value,
                   c.ValueType
               });
            return Task.FromResult<IResult>(TypedResults.Json(roles));
        }

    }
}
