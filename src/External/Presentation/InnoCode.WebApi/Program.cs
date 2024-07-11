using System;
using System.Threading;
using FastEndpoints;
using FastEndpoints.Swagger;
using InnoCode.AppIdentityService;
using InnoCode.Application;
using InnoCode.Domain.Entities;
using InnoCode.PostgresSql;
using InnoCode.PostgresSql.Data;
using InnoCode.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configs = builder.Configuration;

// Add services to the container.
ApplicationDependencyInjection.Config(services, configs);
PostgresSqlDependencyInjection.Config(services, configs);
AppIdentityServiceDependencyInjection.Config(services, configs);
WebApiDependencyInjection.Config(services, configs);

var app = builder.Build();

// Data seeding.
await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.TryResolve<Lazy<InnoCodeContext>>();

    // Can database be connected.
    var canConnect = await context.Value.Database.CanConnectAsync();

    // Database cannot be connected.
    if (!canConnect)
    {
        throw new HostAbortedException(message: "Cannot connect database.");
    }

    // Try seed data.
    var seedResult = await EntityDataSeeding.SeedAsync(
        context: context,
        userManager: scope.TryResolve<Lazy<UserManager<UserEntity>>>(),
        roleManager: scope.TryResolve<Lazy<RoleManager<RoleEntity>>>(),
        cancellationToken: CancellationToken.None
    );

    // Data cannot be seed.
    if (!seedResult)
    {
        throw new HostAbortedException(message: "Database seeding is false.");
    }
}

// Configure the HTTP request pipeline.
app.UseCors()
    .UseAuthentication()
    .UseAuthorization()
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
