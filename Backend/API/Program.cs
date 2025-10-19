using API.Extensions;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCorsPolicy();
builder.Services.AddServiceRegister();
builder.Services.AddMappings();
builder.Services.AddDatabase(configuration);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCorsPolicy();
app.UseMiddleware<DatabaseConnection>();

app.Run();