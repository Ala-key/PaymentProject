using System.Reflection;
using System.Text;
using Core.Services;
using DataLayer.Data;
using DataLayer.Infrastructure;
using DataLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PaymentProject.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((context, settings) =>
{
    settings.ReadFrom.Configuration(context.Configuration);
});

builder.Services.Configure(builder.Configuration);
builder.Services.AddJwtAuthorization(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigurationSwagger();
var app = builder.Build();

//Apply migrations and Seeding.
await app.Services.ApplyMigrationsAsync();
await app.FillRolesAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
