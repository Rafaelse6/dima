using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers
{
    public class CategoryHandler(AppDbContext context) : ICategoryHandler
    {
        public Task<Response<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
        {
            try
            {
                var category = new Category
                {
                    UserId = request.UserId,
                    Title = request.Title,
                    Description = request.Description,
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return new Response<Category?>(category, 201, "Categoria criada com sucesso");
            }
            catch
            {
                return new Response<Category?>(null, 500, "Não foi possível criar a categoria");
            }
        }

        public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
        {
            try
            {
                var category = await context
               .Categories
               .FirstOrDefaultAsync(x => x.Id == request.Id & x.UserId == request.UserId);

                if (category is null)
                    return new Response<Category?>(null, 404, "Categoria não encontrada");
                category.Title = request.Title;
                category.Description = request.Description;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return new Response<Category?>(category);
            }
            catch
            {
                return new Response<Category?>(null, 500, "[FP079] Não foi possível atualizar a categoria");
            }
        }

        public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
        {
            try
            {
                var category = await context
               .Categories
               .FirstOrDefaultAsync(x => x.Id == request.Id & x.UserId == request.UserId);

                if (category is null)
                    return new Response<Category?>(null, 404, "Categoria não encontrada");

                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return new Response<Category?>(category, message: "Categoria excluida com sucesso");
            }
            catch
            {
                return new Response<Category?>(null, 500, "[FP079] Não foi possível deletar a categoria");
            }
        }
    }
}
