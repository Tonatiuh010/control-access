using Engine.DAL;
using Engine.Constants;
using ControlAccess.Hubs;
using System;
using System.Reflection;
using System.IO;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(
//        policy =>
//        {
//            policy.WithOrigins("*", "localhost:*", "localhost");
//        });
//});

// Add services to the container.
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        }));
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("AccessControl", new OpenApiInfo
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

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<CheckHub>("/CheckMonitor").RequireCors("CorsPolicy");

});

app.Run();
