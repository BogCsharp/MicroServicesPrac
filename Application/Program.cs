
using Microsoft.AspNetCore.Builder;
using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder
    .AddBearerAuthentication()
    .AddOptions()
    .AddSwagger()
    .AddData()
    .AddApplicationServices()
    .AddIntegrationServices();


var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();
