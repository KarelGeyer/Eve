using System.Text;
using Common.Shared.Interfaces;
using Common.Shared.Models;
using EmailService;
using Eve.API.Extensions;
using Eve.API.Middlewares;
using Eve.API.ServiceAccessors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Users.Application.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

//*** Manage Secrets ***//
// Naplnìní ExternalSettings -> User.Application
builder.Services.Configure<ExternalSettings>(builder.Configuration.GetSection("ABSTRACT_API"));

//*** Rate Limiting ***//
builder.Services.AddAppRateLimiting(builder.Configuration);

//*** Email Klient ***//
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));
builder.Services.AddTransient<IEmailService, MailService>();

//*** JWT Authentication ***//
builder.Services.AddJwtAuthentication(builder.Configuration);

//*** Middleware ***//
builder.Services.AddHttpContextAccessor();

//*** DI ***//
// Add detection
builder.Services.AddDetection();

// Add modules services
builder.Services.AddModulesServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // UI For Testing API Endpoints
    app.MapScalarApiReference();
}

//*** Middleware ***//
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<HeadersMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();

app.UseDetection();

app.UseHttpsRedirection();
app.UseRouting();

app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
