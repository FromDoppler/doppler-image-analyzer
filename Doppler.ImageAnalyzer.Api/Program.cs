using Doppler.ImageAnalyzer.Api.Logging;
using Doppler.ImageAnalyzer.Api.Services.Repositories;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddJsonFile("/run/secrets/appsettings.Secret.json", true);

// Add services to the container.
var appConfig = builder.Configuration.GetConfiguration<AppConfiguration>();
builder.Services.AddDopplerSecurity();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMongoDBRepositoryService(builder.Configuration);
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter the token into field as 'Bearer {token}'",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme },
                            },
                            Array.Empty<string>()
                        }
                    });

    var baseUrl = builder.Configuration.GetValue<string>("BaseURL");
    if (!string.IsNullOrEmpty(baseUrl))
    {
        c.AddServer(new OpenApiServer() { Url = baseUrl });
    };
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddOperationsLogic(appConfig);
builder.Host.UseSerilog((hostContext, loggerConfiguration) =>
{
    loggerConfiguration.SetupSeriLog(hostContext.Configuration, hostContext.HostingEnvironment);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }