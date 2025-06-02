using Microsoft.AspNetCore.Builder;
using ChatAI.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();

