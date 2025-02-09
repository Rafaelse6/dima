using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories
{
    public class GetCategoryByIdEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/{Id}", HandleAsync)
          .WithName("Categories: Get")
          .WithSummary("Busca uma categoria pelo seu Id")
          .WithDescription("Busca uma categoria pelo seu Id")
          .WithOrder(4)
          .Produces<Response<Category?>>();


        private static async Task<IResult> HandleAsync(ICategoryHandler handler, long id)
        {
            var request = new GetCategoryByIdRequest
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