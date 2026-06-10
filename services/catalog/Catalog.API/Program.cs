using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Context;
using Catalog.Infrastructure.Repositories;
using Common.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog(Logging.ConfigureLogger);

builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://identityserver:9011";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidAudience = "Catalog",
            ValidIssuer = "http://identityserver:9011",
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero

        };


        //Add this to docker to host communtication
        options.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"======= AUTHENTICTION FAILED");
                Console.WriteLine($"Exception :{context.Exception.Message}");
                Console.WriteLine($"Authority:{options.Authority}");
                return Task.CompletedTask;
            },
        OnTokenValidated = context =>
        {
            Console.WriteLine("TOKEN VALID");
            return Task.CompletedTask;
        },

    OnChallenge = context =>
    {
        Console.WriteLine($"CHALLENGE ERROR: {context.Error}");
        Console.WriteLine($"DESCRIPTION: {context.ErrorDescription}");
        return Task.CompletedTask;
    }
        };


    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanRead",
        policy => policy.RequireClaim("scope", "catalogapi.read"));
});
var userPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser().Build();

builder.Services.AddControllers(config =>
{
    config.Filters.Add(new AuthorizeFilter(userPolicy));
});
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<ProductMappingProfile>();
});

builder.Services.AddMediatR(cfg =>
cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(GetProductByIdQuery))));

builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBrandRepository, ProductRepository>();
builder.Services.AddScoped<ITypeRepository, ProductRepository>();
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
        Title = "Catalog API",
        Version = "v1",
        Description = "this is API Catalog in ecommerce application",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Yousef Mohamed",
            Email = "youaefmahmed481@gmail.com",
            Url = new Uri("https://yourwebsite")
        }
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
// Learn mor
builder.Services.AddOpenApi();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // BEFORE UseSwagger / routing
    app.Use((ctx, next) =>
    {
        if (ctx.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var p) && !string.IsNullOrEmpty(p))
            ctx.Request.PathBase = p.ToString();   // e.g., "/catalog"
        return next();
    });

    app.UseSwagger(c =>
    {
        // Make the OpenAPI "servers" base path match the prefix so Try it out uses /catalog/...
        c.PreSerializeFilters.Add((doc, req) =>
        {
            var prefix = req.Headers["X-Forwarded-Prefix"].FirstOrDefault();
            if (!string.IsNullOrEmpty(prefix))
                doc.Servers = new List<Microsoft.OpenApi.Models.OpenApiServer>
            { new() { Url = prefix } };
        });
    });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "Catalog.API v1"); // relative path (no leading '/')
        c.RoutePrefix = "swagger";
    });

    app.UseSwagger();
    app.UseSwaggerUI();
}
// app.UseCors("CorsPolicy");
//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
