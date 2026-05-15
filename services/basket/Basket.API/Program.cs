using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<BasketMappingProfile>();
});




builder.Services.AddMediatR(cfg =>
cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(),
Assembly.GetAssembly(typeof(GetBasketByUserNameQuery))));
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddApiVersioning(option =>
{
    option.ReportApiVersions = true;
    option.AssumeDefaultVersionWhenUnspecified = true;
    option.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Basket API",
        Version = "v1",
        Description = "this is API Basket in ecommerce application",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Yousef Mohamed",
            Email = "youaefmahmed481@gmail.com",
            Url = new Uri("https://yourwebsite")
        }
    });
}
    );
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddStackExchangeRedisCache(options =>
{
    var redisConnection =
        builder.Configuration["RedisCacheSetting:ConnectionString"];

    options.ConfigurationOptions =
        ConfigurationOptions.Parse(redisConnection!);

    options.ConfigurationOptions.AbortOnConnectFail = false;
});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapOpenApi();
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
