﻿using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Transactions
{
    public class GetTransactionByPeriodEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapGet("/", HandleAsync)
           .WithName("Transactions: Get All")
           .WithSummary("Recupera todas as transações")
           .WithDescription("Recupera todas as transações")
           .WithOrder(5)
           .Produces<PagedResponse<List<Transaction>?>>();

        private static async Task<IResult> HandleAsync(ClaimsPrincipal user,
                                                        ITransactionHandler handler,
                                                       [FromQuery] DateTime? startDate = null,
                                                       [FromQuery] DateTime? endDate = null,
                                                       [FromQuery] int pageSize = Configuration.DefaultPageSize,
                                                       [FromQuery] int pageNumber = Configuration.DefaultPageNumber)
        {
            var request = new GetTransactionsByPeriodRequest
            {
                UserId = user.Identity?.Name ?? string.Empty,
                PageNumber = pageNumber,
                PageSize = pageSize,
                StartDate = startDate,
                EndDate = endDate
            };

            var result = await handler.GetTransactionsByPeriod(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
