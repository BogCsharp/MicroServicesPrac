
using Microsoft.AspNetCore.Builder;
using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder
    .AddSwagger()
    .AddData()
    .AddApplicationServices()
    .AddIntegrationServices();


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
