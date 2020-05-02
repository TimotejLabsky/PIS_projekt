using System.Net.Mail;
using AutoMapper;
using FiitCalendarService;
using FiitCustomerService;
using FiitEmailService;
using FiitTaskList;
using FiitValidatorService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Pis.Projekt.Business;
using Pis.Projekt.Business.Authorization;
using Pis.Projekt.Business.Notifications;
using Pis.Projekt.Business.Notifications.Domain;
using Pis.Projekt.Business.Scheduling;
using Pis.Projekt.Domain.Database.Contexts;
using Pis.Projekt.Domain.Mappings;
using Pis.Projekt.Domain.Repositories;
using Pis.Projekt.Domain.Repositories.Impl;
using Pis.Projekt.Framework;
using Pis.Projekt.Framework.Email.Impl;

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
            services.Configure<WsdlConfiguration<WsdlEmailClient>>(
                _configuration.GetSection("EmailService:WSDL"));
            services.Configure<WsdlConfiguration<WsdlTaskClient>>(
                _configuration.GetSection("TaskService:WSDL"));
            services.Configure<WsdlEmailClientConfiguration>(
                _configuration.GetSection("EmailService"));
            services.Configure<WsdlConfiguration<WsdlTaskClient>>(
                _configuration.GetSection("TaskService"));
            services.Configure<SmtpClientConfiguration>(
                _configuration.GetSection("EmailService:SMTP"));
            services.Configure<WaiterService.WaiterConfiguration>(
                _configuration.GetSection("WaiterService"));
            services.Configure<NotificationConfiguration>(
                _configuration.GetSection("NotificationService"));
            services.AddDbContext<SalesDbContext>(c => c.UseInMemoryDatabase("sales"));
            services.AddScoped<SalesOptimalizationService>();
            services.AddScoped<ISalesAggregateRepository, SalesAggregateRepository>();
            services.AddScoped<IPricedProductRepository, PricedProductRepository>();
            services.AddScoped<ProductPersistenceService>();
            services.AddScoped<NotificationFactory>();
            services.AddScoped<IOptimizationNotificationService, EmailNotificationService>();
            services
                .AddScoped<INotificationClient<IEmailNotification, IEmail>, SmtpClientAdapter>();
            services.AddScoped<INotificationClient<IEmailNotification, IEmail>, WsdlEmailClient>();
            services.AddScoped<SalesEvaluatorService>();
            services.AddScoped<WaiterService>();
            services.AddScoped<WeekCounter>();
            services.AddScoped<SmtpClient>();
            services.AddSingleton<AuthorizationService>();
            services.AddSingleton<CustomerPortTypeClient>();
            services.AddSingleton<EmailPortTypeClient>();
            services.AddSingleton<CalendarPortTypeClient>();
            services.AddSingleton<TaskListPortTypeClient>();
            services.AddSingleton<ValidatorPortTypeClient>();
            services.AddAutoMapper(c =>
            {
                c.AddProfile<PricedProductProfile>();
                c.AddProfile<ProductProfile>();
                c.AddProfile<SalesAggregateProfile>();
            }, typeof(Startup));
            services.AddLogging(c => c.AddConsole().AddConfiguration(_configuration));
            services.AddControllers();
            
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sales Optimization API", Version = "v1" });
            });

            // private readonly CronSchedulerService _cronScheduler;
            // private readonly TaskSchedulerService _taskScheduler;
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
                c.RoutePrefix = string.Empty;  // Set Swagger UI at apps root
            });

            app.UseMiddleware<AuthorizationMiddleWare>();
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private readonly IConfiguration _configuration;
    }
}