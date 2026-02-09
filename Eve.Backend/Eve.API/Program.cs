using Common.Shared.Interfaces;
using EmailService;
using Eve.API.Extensions;
using Eve.API.Middlewares;
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

//*** Middleware ***//
builder.Services.AddHttpContextAccessor();

//*** DI ***//

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // UI For Testing API Endpoints
    app.MapScalarApiReference();
}

//*** Middleware ***//
app.UseMiddleware<HeadersMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();

app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
