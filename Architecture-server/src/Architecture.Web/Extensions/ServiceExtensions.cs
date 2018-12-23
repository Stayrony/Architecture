using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Architecture.Model.Shared;
using Architecture.Web.DocumentFilters;
using Architecture.Web.OperationFilters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace Architecture.Web.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerExamples();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", CreateSwaggerDocInfo());

                options.OperationFilter<AddEnumOperationFilter>();
                options.OperationFilter<AuthorizationHeaderOperationFilter>();

                options.DocumentFilter<SetVersionInPaths>();

                options.ExampleFilters();

                options.OperationFilter<AddFileParamTypesOperationFilter>(); // Adds an Upload button to endpoints which have [AddSwaggerFileUploadButton]
                options.OperationFilter<AddResponseHeadersFilter>(); // [SwaggerResponseHeader]
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>(); // Adds "(Auth)" to the summary so that you can see which endpoints have Authorization
                                                                                    // or use the generic method, e.g. c.OperationFilter<AppendAuthorizeToSummaryOperationFilter<MyCustomAttribute>>();

#if !TEST
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "Architecture.Web.xml");

                options.IncludeXmlComments(xmlPath);
#endif
            });
        }

        public static void ConfigureJwt(this IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            ITokenService tokenService = serviceProvider.GetRequiredService<ITokenService>();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = tokenService.TokenInfo.ValidateIssuer,
                        ValidateLifetime = tokenService.TokenInfo.ValidateLifetime,
                        ValidateIssuerSigningKey = tokenService.TokenInfo.ValidateIssuerSigningKey,
                        ValidateAudience = tokenService.TokenInfo.ValidateAudience,

                        ValidIssuer = tokenService.TokenInfo.Issuer,
                        ValidAudience = tokenService.TokenInfo.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenService.TokenInfo.IssuerSecurityKey))
                    };

                    // We have to hook the OnMessageReceived event in order to
                    // allow the JWT authentication handler to read the access
                    // token from the query string when a WebSocket or 
                    // Server-Sent Events request comes in.
                    cfg.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/ChatHub")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        private static Info CreateSwaggerDocInfo()
        {
            var info = new Info()
            {
                Title = $"Architecture API Developer Documentation",
                Version = "v1",
                Description = $"The REST APIs provide programmatic access to read and write Architecture data.\n" +
                                 $"<a href='' target='_blank'>Error Codes Glossary</a> ",
                Contact = new Contact() { Name = "Stayrony Inc.", Email = "ana.mizumskaya@gmail.com" },
            };

            return info;
        }
    }
}
