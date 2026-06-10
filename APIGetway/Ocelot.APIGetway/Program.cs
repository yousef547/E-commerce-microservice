using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var authSchema = "EShoppingGatewayAuthSchema";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(authSchema, options =>
    {
        options.Authority = "http://identityserver:9011";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "http://identityserver:9011",
            ValidateAudience = true,
            ValidAudience = "EShoppingGateway",
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
builder.Configuration
       .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);
//builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello ocelot"); });
});
//app.UseSwaggerForOcelotUI();
await app.UseOcelot();

await app.RunAsync();
