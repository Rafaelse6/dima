using Dima.Api.Data;
using Dima.Api.Handlers;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var cnnStr = builder
    .Configuration
    .GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(cnnStr);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.CustomSchemaIds(n => n.FullName);
});
builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();

// Add services to the container.

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.

app.MapPost("/v1/categories", async (CreateCategoryRequest request, ICategoryHandler handler) =>
    await handler.CreateAsync(request))
    .WithName("Categories: Create")
    .WithSummary("Cria uma nova categoria")
    .Produces<Response<Category?>>();

app.MapPut("/v1/categories/{id}", async (long id, UpdateCategoryRequest request, ICategoryHandler handler) =>
    {
        request.Id = id;
        return await handler.UpdateAsync(request);
    })
        .WithName("Categories: Update")
        .WithSummary("Atualiza uma nova categoria")
        .Produces<Response<Category?>>();

app.MapDelete("/v1/categories/{id}", async (long id, ICategoryHandler handler) =>
    {
        var request = new DeleteCategoryRequest
        {
            Id = id,
            UserId = "teste@balta.io"
        };
        return await handler.DeleteAsync(request);
    })
        .WithName("Categories: Delete")
        .WithSummary("Deleta uma nova categoria")
        .Produces<Response<Category>>();

app.MapGet("/v1/categories/{id}", async (long id, ICategoryHandler handler) =>
{
    var request = new GetCategoryByIdRequest
    {
        Id = id,
        UserId = "teste@balta.io"
    };
    return await handler.GetByIdAsync(request);
})
        .WithName("Get By Id")
        .WithSummary("Retorna uma Categoria")
        .Produces<Response<Category>>();

app.Run();
