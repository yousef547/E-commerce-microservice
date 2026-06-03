using Asp.Versioning;
using Basket.Application.GrpcServices;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Common.Logging;
using Discount.Grpc.Protos;
using MassTransit;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

AppContext.SetSwitch(
    "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport",
    true);
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog(Logging.ConfigureLogger);

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
builder.Services.AddScoped<DiscountGrpcService>();
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
    cfg => cfg.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));

builder.Services.AddApiVersioning(option =>
{
    option.ReportApiVersions = true;
    option.AssumeDefaultVersionWhenUnspecified = true;
    option.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddEndpointsApiExplorer();

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

    options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Basket API",
        Version = "v2",
        Description = "This is API for basket microservice v2 in ecommerce application",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Yousef Mohamed",
            Email = "youaefmahmed481@gmail.com",
            Url = new Uri("https://yourwebsite.eg")
        }
    });

    options.DocInclusionPredicate((version, apiDescrption) =>
    {
        if (!apiDescrption.TryGetMethodInfo(out var methodInfo))
        {
            return false;
        }

        var versions = methodInfo.DeclaringType?
                       .GetCustomAttributes(true)
                       .OfType<ApiVersionAttribute>()
                       .SelectMany(attr => attr.Versions);

        return versions?.Any(v => $"v{v.ToString()}" == version) ?? false;

    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: **Bearer {your JWT}**",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
}
    );
Console.WriteLine(
    builder.Configuration["RedisCacheSetting:ConnectionString"]);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddStackExchangeRedisCache(options =>
{
    var redisConnection =
        builder.Configuration["RedisCacheSetting:ConnectionString"];

    options.ConfigurationOptions =
        ConfigurationOptions.Parse(redisConnection!);

    options.ConfigurationOptions.AbortOnConnectFail = false;
});
builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ct, cfg) =>
    {

        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});
builder.Services.AddMassTransitHostedService();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapOpenApi();
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Use *relative* URLs so the /basket prefix is preserved by the browser
    c.SwaggerEndpoint("v1/swagger.json", "Basket.API v1");   // no leading '/'
    c.SwaggerEndpoint("v2/swagger.json", "Basket.API v2");   // no leading '/'
    c.RoutePrefix = "swagger";

}); ;

app.UseAuthorization();

app.MapControllers();

app.Run();
