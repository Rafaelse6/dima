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

app.MapPost("/v1/categories", (CreateCategoryRequest request, ICategoryHandler handler) =>
    handler.CreateAsync(request))
    .WithName("Categories: Create")
    .WithSummary("Cria uma nova categoria")
    .Produces<Response<Category>>();


app.Run();
