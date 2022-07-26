using Engine.DAL;
using Engine.Constants;
using System;
using System.Reflection;
using System.IO;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("AccessControl", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "AccessControl",
        Version = "v1"
    });
});

var app = builder.Build();

app.MapGet("/", () => "Control Access API is working...");

ControlAccessDAL.ConnString = builder.Configuration.GetConnectionString(C.CTL_ACC);
ControlAccessDAL.SetOnConnectionException((ex, msg) => Console.WriteLine($"Error Opening connection {msg} - {ex.Message}"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

// app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
