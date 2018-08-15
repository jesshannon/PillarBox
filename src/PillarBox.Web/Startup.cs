using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PillarBox.Business.Services.Common;
using PillarBox.Business.Services.Messages;
using PillarBox.Business.Services.Notifcations;
using PillarBox.Business.Services.Smtp;
using PillarBox.Data;
using PillarBox.Web.Controllers;
using PillarBox.Web.Init;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using PillarBox.Data.Config;
using PillarBox.Business.Services.Filters;
using Microsoft.EntityFrameworkCore;

namespace PillarBox.Web
{
    public class Startup
    {
        private MapperConfiguration _mapperConfiguration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            using (var client = new PillarBoxContext())
            {
                client.Database.Migrate();
            }

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddPolicy("CorsPolicy", builder => {
                    builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://localhost:4200");
                }));
            services.AddSignalR();

            services.AddMvc();
            services.AddLogging();

            services.Configure<ServerDefaults>(Configuration.GetSection("ServerDefaults"));

            services.AddDbContext<PillarBoxContext>();

            services.AddSingleton<IMapper>(Mapper.Instance);

            services.AddSingleton<IHostedService, SmtpListener>();
            services.AddSingleton<INotificationDespatcher, NotifcationDespatcher>();
            
            services.AddTransient<IMessageHandler, MessageHandler>();
            services.AddTransient<IInboxService, InboxService>();
            services.AddTransient<IMessagesService, MessagesService>();
            services.AddScoped<IMessageContext, MessageContext>();
            services.AddTransient<ISmtpAuthenticate, SmtpAuthenticate>();
            services.AddTransient<IUserContext, UserContext>();
            services.AddTransient<IMessageRuleService, MessageRuleService>();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors("CorsPolicy");

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "DefaultApi",
                    template: "api/{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSignalR(routes => {
                routes.MapHub<MessageHub>("/notifications");
            });

			app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
