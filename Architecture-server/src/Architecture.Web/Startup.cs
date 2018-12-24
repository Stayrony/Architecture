using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using Architecture.Model.Database.Options;
using Architecture.Model.Invoke;
using Architecture.Model.Database.EnumMappers;
using Architecture.View.Response;
using Architecture.View;
using AutoMapper;
using Architecture.Model.Shared;
using Architecture.Model.Database.Shared;
using NLog.Extensions.Logging;
using NLog.Web;
using Architecture.Web.Hubs;
using Architecture.Web.Extensions;

namespace Architecture.Web
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //options
            services.AddDataProtection();
            services.Configure<JwtTokenOptions>(Configuration.GetSection("JwtToken"));
            services.Configure<SendGridOptions>(Configuration.GetSection("SendGrid"));
            services.Configure<FilesOptions>(Configuration.GetSection("Files"));
            services.Configure<UrlsOptions>(Configuration.GetSection("Urls"));

            services.AddTransient<EntityToModelProfile>();

            var connectionString = Configuration.GetConnectionString("ArchitectureContext");

            services.AddOptions();
            services.AddScoped(typeof(IInvokeHandler<>), typeof(BaseInvokeHandler<>));
            services.AddSingleton<IInvokeResultSettings, InvokeResultSettings>();
            services.AddMvc();

            services.AddTransient<IViewMapper, ResponseViewMapper>();
            services.AddTransient<ITokenService, TokenService>();

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            //register logger
            services.AddSingleton<Serilog.ILogger>(
                                                   new LoggerConfiguration()
                                                       .ReadFrom.Configuration(Configuration)
                                                       .CreateLogger());
            services.ConfigureSwagger();
            //services.AddEntityFrameworkNpgsql().AddDbContext<ArchitectureDbContext>(options => options.UseNpgsql(connectionString));
            //services.AddScoped(typeof(SeedDatabase));
            //services.AddScoped(typeof(TestSeedDatabase));

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            Mapper.Reset();
            services.AddAutoMapper(typeof(Startup).Assembly);

            var entityToModelProfile = serviceProvider.GetRequiredService<EntityToModelProfile>();
            services.AddSingleton(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ModelToViewMapperProfile());
                cfg.AddProfile(entityToModelProfile);
            }).CreateMapper());

            services.ConfigureJwt();
            services.AddSignalR();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env,
                              ILoggerFactory loggerFactory
                              //TestSeedDatabase testSeedDatabese,
                              //SeedDatabase seedDatabase
                              )
        {
            env.ConfigureNLog("nlog.config");
            loggerFactory.AddNLog();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/ChatHub");
            });

            //AUTOCUT-S
            //seedDatabase.Seed();
            //if (env.IsDevelopment()
            //    || env.IsEnvironment("Dev"))
            //{
            //    testSeedDatabese.Seed();
            //}

            //app.UseStaticFiles();

            //if (env.IsDevelopment())
            //{
            //    app.UseStaticFiles(new StaticFileOptions()
            //    {
            //        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
            //        RequestPath = new PathString("/uploads")
            //    });
            //}
            //else
            //{
            //    app.UseStaticFiles(new StaticFileOptions()
            //    {
            //        FileProvider = new PhysicalFileProvider(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "uploads")),
            //        RequestPath = new PathString("/uploads")
            //    });
            //}

            //AUTOCUT-F

            //TODO add only friendly origins
            app.UseCors(options =>
            {
                options.AllowAnyMethod();
                options.AllowAnyHeader();
                options.AllowAnyOrigin();
            });

            if (env.IsDevelopment()
             || env.IsEnvironment("Dev"))
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.InjectStylesheet("/swagger-ui/custom.css");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
            });

            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();

            app.UseMvc();
        }
    }
}
