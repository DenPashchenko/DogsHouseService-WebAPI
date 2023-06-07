using DogsHouseService.Application.Common.Mappings;
using DogsHouseService.Application.Interfaces;
using DogsHouseService.Persistence;
using DogsHouseService.WebApi.Middleware.Extensions;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddAutoMapper(config =>
{
	config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
	config.AddProfile(new AssemblyMappingProfile(typeof(IAppDbContext).Assembly));
});
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(IAppDbContext).Assembly));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
	var version = Assembly.GetExecutingAssembly().GetName().Version;
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	config.IncludeXmlComments(xmlPath);
	config.DescribeAllParametersInCamelCase();
	config.SwaggerDoc($"v{version?.Major}", new OpenApiInfo 
	{ 
		Title = "DogsHouseService API", Version = $"v{version?.Major}.{version?.Minor}.{version?.Build}" 
	});
});
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCustomExceptionHandler();

app.UseHttpsRedirection();

app.UseRateLimiting();

app.MapControllers();

app.Run();
