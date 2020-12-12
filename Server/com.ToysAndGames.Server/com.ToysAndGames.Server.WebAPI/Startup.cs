using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.ToysAndGames.Server.WebAPI.DAL.Context;
using com.ToysAndGames.Server.WebAPI.DAL.Models;
using com.ToysAndGames.Server.WebAPI.DAL.Repositories.Generic;
using com.ToysAndGames.Server.WebAPI.ModelValidation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace com.ToysAndGames.Server.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApiContext>();

            //Incorporates FluentValidation into controllers.
            services.AddControllers().AddFluentValidation();

            //Adds custom Product validation rules
            services.AddTransient<IValidator<Product>, ProductValidator>();

            //Adds Repositories IoC
            services.AddScoped<IUnitOfWork, GenericUnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //Allows all origins, methods and headers. For test purposes only.
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
