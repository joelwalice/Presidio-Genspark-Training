using BlobAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<BlobStorageService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.mapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();