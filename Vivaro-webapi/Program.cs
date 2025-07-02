
using System.Globalization;
using Vivaro_webapi.Modules;
using System.Text.Json.Serialization;
using Application.Shared.Convertes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

CultureInfo.CurrentCulture = new CultureInfo("en-US");
CultureInfo.CurrentUICulture = new CultureInfo("en-US");

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    WebRootPath = "wwwroot",
    Args = args
});
builder.Services.AddHealthChecks();
builder.Services.ConfigureDataBase(builder.Configuration);
builder.Services.AddUseCases(builder.Configuration);
builder.Services.AddApiVersion();
builder.Services.ConfigureKeyCloak(builder.Configuration);

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(p => p.AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod()
    );
});

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
        opt.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());
        opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    _ = app.ConfigureSwaggerUi(app.Services.GetRequiredService<IApiVersionDescriptionProvider>());
    app.Services.CreateDatabase();
}

app.MapHealthChecks("/healthcheck");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();