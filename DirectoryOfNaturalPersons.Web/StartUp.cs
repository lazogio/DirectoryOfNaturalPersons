using System.Resources;
using System.Text.Json.Serialization;
using DirectoryOfNaturalPersons.Application;
using DirectoryOfNaturalPersons.Application.Interface;
using DirectoryOfNaturalPersons.Application.PipelineBehaviour;
using DirectoryOfNaturalPersons.Application.Profiles;
using DirectoryOfNaturalPersons.Application.ResourceManagerService;
using DirectoryOfNaturalPersons.Domain.Interface;
using DirectoryOfNaturalPersons.Middlewares;
using DirectoryOfNaturalPersons.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

public class StartUp
{
    public static WebApplication InitializeApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);
        var app = builder.Build();
        Configure(app);
        return app;
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        var applicationAssembly = typeof(AssemblyReference).Assembly;
        var connectionString = builder.Configuration.GetConnectionString("DirectoryOfNaturalPersonsDB");

        builder.Services
            .AddControllers(options => { options.Filters.Add<ValidationActionFilter>(); })
            .AddApplicationPart(applicationAssembly)
            .AddDataAnnotationsLocalization()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Directory Of Natural Persons",
                Version = "v1"
            });
            c.OperationFilter<AcceptLanguageMiddleware.CustomHeaderSwaggerAttribute>();
        });

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddMediatR(applicationAssembly);
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        builder.Services.AddValidatorsFromAssembly(applicationAssembly);
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString).EnableSensitiveDataLogging());
        builder.Services.AddScoped<ValidationActionFilter>();

        builder.Services.AddScoped<IPersonRepository, PersonRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddAutoMapper(typeof(CreateProfile).Assembly);
        
        builder.Services.AddSingleton<IResourceManagerService>(_ =>
        {
            var resourceManager = new ResourceManager("DirectoryOfNaturalPersons.Application.Resources.SharedResource", applicationAssembly);
            return new ResourceManagerService(resourceManager);
        });

        builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
    }

    private static void Configure(WebApplication app)
    {
        var dbInitializer = new DbInitializer();
        dbInitializer.SeedAsync(app.Services, CancellationToken.None).Wait();

        app.UseMiddleware<AcceptLanguageMiddleware>();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}