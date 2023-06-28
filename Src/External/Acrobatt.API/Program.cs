using Acrobatt.Application;
using Acrobatt.Infrastructure;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o => o.AddPolicy("defaultPolicy", policyBuilder =>
{
    policyBuilder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

if (!builder.Environment.IsDevelopment())
{
    string? port = Environment.GetEnvironmentVariable("PORT");
    builder.WebHost.UseUrls("http://*:" + port);
}


// Add services to the container
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();
builder.Services.AddAuthorization();

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

// Swagger configuration for Open API Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc ("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Acrobatt API",
        Description = "An ASP.NET Core Web API for Acrobatt Project",
    });
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
});


WebApplication app = builder.Build();

// Use middlewares
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCors("defaultPolicy");

app.MapControllers();
app.Run();

namespace Acrobatt.API
{
    public partial class Program { }
}