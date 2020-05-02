using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pis.Projekt.Business;
using Pis.Projekt.Business.Authorization;
using Pis.Projekt.Business.Notifications;
using Pis.Projekt.Business.Scheduling;
using Pis.Projekt.Domain.Database.Contexts;
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
            services.Configure<WsdlConfiguration<WsdlEmailClient>>(_configuration.GetSection("EmailService:WSDL"));
            services.Configure<WsdlConfiguration<WsdlTaskClient>>(_configuration.GetSection("TaskService:WSDL"));
            services.Configure<WsdlEmailClientConfiguration>(_configuration.GetSection("EmailService"));
            services.Configure<WsdlTaskClientConfiguration>(_configuration.GetSection("TaskService"));
            services.Configure<SmtpClientConfiguration>(_configuration.GetSection("EmailService:SMTP"));
            services.AddDbContext<SalesDbContext>(c => c.UseInMemoryDatabase("sales"));
            services.AddScoped<EmailNotificationService>();
            services.AddScoped<SalesOptimalizationService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<AuthorizationMiddleWare>();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/",
                    async context => { await context.Response.WriteAsync("Hello World!"); });
            });
        }

        private readonly IConfiguration _configuration;
    }
}