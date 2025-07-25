using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using Microsoft.OpenApi.Models;
using VideoUploadAPI.Models;
using VideoUploadAPI.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VideoContexts>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddSingleton(x =>
{
    var blobConnectionString = builder.Configuration.GetConnectionString("AzureBlobStorage");
    return new BlobServiceClient(blobConnectionString);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Training Video Portal API", Version = "v1" });
});
builder.Services.AddCors(options=>{
    options.AddDefaultPolicy(policy=>{
        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Training Video Portal API V1");
    });
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
