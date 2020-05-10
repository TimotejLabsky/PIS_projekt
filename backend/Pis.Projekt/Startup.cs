using System.Net.Mail;
using AutoMapper;
using FiitCalendarService;
using FiitCustomerService;
using FiitEmailService;
using FiitTaskList;
using FiitValidatorService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Pis.Projekt.Business;
using Pis.Projekt.Business.Authorization;
using Pis.Projekt.Business.Calendar;
using Pis.Projekt.Business.Notifications;
using Pis.Projekt.Business.Notifications.Domain;
using Pis.Projekt.Business.Notifications.Domain.Impl;
using Pis.Projekt.Business.Scheduling;
using Pis.Projekt.Business.Scheduling.Impl;
using Pis.Projekt.Business.Validation;
using Pis.Projekt.Domain.Database.Contexts;
using Pis.Projekt.Domain.Mappings;
using Pis.Projekt.Domain.Repositories;
using Pis.Projekt.Domain.Repositories.Impl;
using Pis.Projekt.Framework;
using Pis.Projekt.Framework.Email.Impl;
using Pis.Projekt.Framework.Seed;
using Quartz;
using Quartz.Impl;

namespace Pis.Projekt
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // reading configuration
            services.Configure<WsdlConfiguration<WsdlEmailClient>>(
                _configuration.GetSection("EmailService:WSDL"));
            services.Configure<WsdlConfiguration<WsdlTaskClient>>(
                _configuration.GetSection("TaskService:WSDL"));
            services.Configure<WsdlEmailClientConfiguration>(
                _configuration.GetSection("EmailService"));
            services.Configure<SmtpClientConfiguration>(
                _configuration.GetSection("EmailService:SMTP"));
            services.Configure<WaiterService.WaiterConfiguration>(
                _configuration.GetSection("WaiterService"));
            services.Configure<NotificationConfiguration<UserTaskRequiredNotification>>(
                _configuration.GetSection("NotificationService:UserTaskNotification"));
            services.Configure<NotificationConfiguration<OptimizationBegunNotification>>(
                _configuration.GetSection("NotificationService:OptimizationBegunNotification"));
            services.Configure<NotificationConfiguration<OptimizationFinishedNotification>>(
                _configuration.GetSection("NotificationService:OptimizationFinishedNotification"));
            services.Configure<EntitySeederConfiguration>(
                _configuration.GetSection("EntitySeederService"));
            services.Configure<CronSchedulerService.CronSchedulerConfiguration>(
                _configuration.GetSection("CronSchedulerService"));

            //validation
            services.AddSingleton<EmailValidationService>();
            services.AddSingleton<ConfigEmailValidation>();

            // db layer
            
            services.AddScoped<ISalesAggregateRepository, SalesAggregateRepository>();
            services.AddScoped<IPricedProductRepository, PricedProductRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<AdvertisedRepository>();
            services.AddScoped<ProductPersistenceService>();
            var dbConnectionString = _configuration.GetValue<string>("Database:ConnectionString");
            services.AddDbContext<SalesDbContext>(c => c.UseSqlServer(dbConnectionString));

            // notification pipeline
            services.AddSingleton<NotificationFactory>();
            services.AddSingleton<IOptimizationNotificationService, EmailNotificationService>();
            services
                .AddSingleton<INotificationClient<IEmailNotification, IEmail>, SmtpClientAdapter>();
            services
                .AddSingleton<INotificationClient<IEmailNotification, IEmail>, WsdlEmailClient>();

            // testing tools
            services.AddSingleton<EntitySeeder>();
            services.AddSingleton<AuthorizationService>();

            // business case
            services.AddSingleton<SalesEvaluatorService>();
            services.AddSingleton<WeekCounter>();
            services.AddSingleton<WaiterService>();
            services.AddSingleton<AggregateFetcher>();
            services.AddSingleton<DecreasedSalesHandler>();
            services.AddSingleton<IncreasedSalesHandler>();
            services.AddSingleton<PriceCalculatorService>();
            services.AddSingleton<UserTaskCollectionService>();
            services.AddSingleton<SalesOptimalizationService>();

            // scheduling
            services.AddSingleton<CronSchedulerService>();
            services.AddSingleton<OptimizationJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // external services
            services.AddSingleton<SupplierService>();

            // WSDL adapters
            services.AddSingleton<ITaskClient, WsdlTaskClient>();
            services.AddSingleton<WsdlCalendarService>();
            services.AddSingleton<SmtpClient>();

            // WSDL references
            services.AddSingleton<CustomerPortTypeClient>();
            services.AddSingleton<EmailPortTypeClient>();
            services.AddSingleton<CalendarPortTypeClient>();
            services.AddSingleton<TaskListPortTypeClient>();
            services.AddSingleton<ValidatorPortTypeClient>();

            // jobs
            services.AddTransient<UserTaskTimeoutEvaluationJob>();
            services.AddTransient<OptimizationJob>();

            services.AddAutoMapper(c =>
            {
                c.AddProfile<PricedProductProfile>();
                c.AddProfile<ProductProfile>();
                c.AddProfile<SalesAggregateProfile>();
                c.AddProfile<ScheduledTaskProfile>();
            }, typeof(Startup));
            services.AddLogging(c => c.AddConsole().AddConfiguration(_configuration));
            services.AddControllers().AddNewtonsoftJson();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo {Title = "Sales Optimization API", Version = "v1"});
            });
            services
                .AddSwaggerGenNewtonsoftSupport(); // explicit opt-in - needs to be placed after AddSwaggerGen()
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty; // Set Swagger UI at apps root
            });

            app.UseCors(c =>
            {
                c.AllowAnyOrigin();
                c.AllowAnyMethod();
                c.AllowAnyHeader();
            });
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.ApplicationServices.GetRequiredService<ConfigEmailValidation>().Validate().Wait();
            app.ApplicationServices.GetRequiredService<EntitySeeder>().Seed().Wait();
            var scheduler = app.ApplicationServices.GetRequiredService<CronSchedulerService>();
            scheduler.StartAsync(default).Wait();
            // scheduler.ScheduleNextOptimalizationTask().Wait();
        }

        private readonly IConfiguration _configuration;
    }
}