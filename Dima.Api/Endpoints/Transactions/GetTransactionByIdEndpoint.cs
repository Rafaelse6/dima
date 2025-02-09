﻿using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions
{
    public class GetTransactionByIdEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/{Id}", HandleAsync)
           .WithName("Transaction: Get")
           .WithSummary("Busca uma transação pelo seu Id")
           .WithDescription("Busca uma transação pelo seu Id")
           .WithOrder(4)
           .Produces<Response<Transaction?>>();

        private static async Task<IResult> HandleAsync( ITransactionHandler handler, long id)
        {
            var request = new GetTransactionByIdRequest
            {
                UserId = "teste@balta.io",
                Id = id
            };

            var result = await handler.GetByIdAsync(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
