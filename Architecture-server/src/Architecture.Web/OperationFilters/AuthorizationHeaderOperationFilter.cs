using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Architecture.Web.OperationFilters
{
    public class AuthorizationHeaderOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Adds an authorization header to the given operation in Swagger.
        /// </summary>
        /// <param name="operation">The Swashbuckle operation.</param>
        /// <param name="context">The Swashbuckle operation filter context.</param>
        void IOperationFilter.Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }

            var authorizeAttributes = context.ApiDescription
                .ControllerAttributes()
                .Union(context.ApiDescription.ActionAttributes())
                .OfType<AuthorizeAttribute>();
            var allowAnonymousAttributes = context.ApiDescription.ActionAttributes().OfType<AllowAnonymousAttribute>();

            if (!authorizeAttributes.Any() && !allowAnonymousAttributes.Any())
            {
                return;
            }

            var parameter = new NonBodyParameter
            {
                Name = "Authorization",
                In = "header",
                Description = "The bearer token",
                Required = true,
                Type = "string"
            };

            operation.Parameters.Add(parameter);
        }
    }
}
