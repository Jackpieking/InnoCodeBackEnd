using System;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Remove all unnessary resources.
GC.Collect();

// Run the app.
await app.RunAsync();
