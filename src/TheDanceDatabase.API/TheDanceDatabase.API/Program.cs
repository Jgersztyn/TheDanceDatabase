using System.Runtime.CompilerServices;
using Scalar.AspNetCore;
using TheDanceDatabase.API;
using TheDanceDatabase.API.Presentation;

[assembly: InternalsVisibleTo("TheDanceDatabase.API.UnitTests")]

var builder = WebApplication.CreateBuilder(args);

// Git-ignored local overrides; env vars and command-line args are re-added so they still win
if (builder.Environment.IsDevelopment())
{
    builder.Configuration
        .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .AddCommandLine(args);
}

builder.Services.AddServices(builder.Configuration);
builder.Services.AddPresentation(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors();

app.MapEndpoints();
app.MapHealthChecks("health-check");

app.Run();
