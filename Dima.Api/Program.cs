using Dima.Api.Data;
using Dima.Api.Endpoints;
using Dima.Api.Handlers;
using Dima.Core.Handlers;
using Microsoft.AspNetCore.Identity;
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

builder.Services.
    AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

builder.Services.AddAuthorization();

builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();


// Add services to the container.

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.

app.MapGet("/", () => new { message = "Ok" });
app.MapEndpoints();

app.Run();
