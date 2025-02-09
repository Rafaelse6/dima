using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories
{
    public class DeleteCategoryEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) =>
            app.MapDelete("/{Id}", HandleAsync)
           .WithName("Categories: Delete")
           .WithSummary("Deleta uma categoria")
           .WithDescription("Deleta categoria existente")
           .WithOrder(3)
           .Produces<Response<Category?>>();


        public static async Task<IResult> HandleAsync(ICategoryHandler handler, long id)
        {
            var request = new DeleteCategoryRequest()
            {
                UserId = "teste@balta.io",
                Id = id
            };

            var result = await handler.DeleteAsync(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }

}