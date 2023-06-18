using HandyCook.API;
using Microsoft.EntityFrameworkCore;
using HandyCook.API.Controllers;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using HandyCook.API.Models.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<HCContext>( options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:HandyCook"]);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

app.Run();
