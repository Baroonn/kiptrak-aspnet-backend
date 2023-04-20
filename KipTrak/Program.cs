using System.Configuration;
using Domain.Models;
using KipTrak.Extensions;
using Service.ServiceContracts;
using Infrastructure.MailService;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreRateLimit;
//using AutoMapper;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureCors(); 
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureBlobService();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAuthentication(); 
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
//builder.Services.AddHttpContextAccessor();
builder.Services.Configure<ApiBehaviorOptions>( options => 
{ 
    options.SuppressModelStateInvalidFilter = true; 
});
builder.Services.ConfigureMailSettings(builder.Configuration);
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureVersioning();
builder.Services.AddControllers()
.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions(); 
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.ConfigureExceptionHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsProduction()) 
    app.UseHsts(); 
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseSwagger(); 
app.UseSwaggerUI(s => 
{ 
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "KipTrak API v1"); 
});

//app.UseHttpsRedirection();
app.UseStaticFiles(); 
app.UseForwardedHeaders(new ForwardedHeadersOptions 
{ 
    ForwardedHeaders = ForwardedHeaders.All 
}); 
app.UseIpRateLimiting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
