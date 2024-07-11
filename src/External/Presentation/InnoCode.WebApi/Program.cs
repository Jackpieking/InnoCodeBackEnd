using System;
using FastEndpoints;
using FastEndpoints.Swagger;
using InnoCode.AppIdentityService;
using InnoCode.PostgresSql;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configs = builder.Configuration;

// Add services to the container.
PostgresSqlDependencyInjection.Config(services, configs);
AppIdentityServiceDependencyInjection.Config(services, configs);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors()
    .UseAuthentication()
    .UseAuthorization()
    .UseSession()
    .UseResponseCaching()
    .UseFastEndpoints()
    .UseSwaggerGen()
    .UseSwaggerUi(options =>
    {
        options.Path = string.Empty;
        options.DefaultModelsExpandDepth = -1;
    });

// Remove all unnessary resources.
GC.Collect();

// Run the app.
await app.RunAsync();
