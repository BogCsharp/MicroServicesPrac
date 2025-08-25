
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpLogging((opt)=>
{
    opt.LoggingFields = HttpLoggingFields.RequestBody | HttpLoggingFields.RequestHeaders | HttpLoggingFields.Duration | HttpLoggingFields.RequestPath |
                      HttpLoggingFields.RequestBody | HttpLoggingFields.RequestHeaders;
});

builder
    .AddBearerAuthentication()
    .AddOptions()
    .AddSwagger()
    .AddData()
    .AddApplicationServices()
    .AddIntegrationServices()
    .AddBackgroundServices();


var app = builder.Build();

app.UseHttpLogging();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();
