using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using NotImplementedException = System.NotImplementedException;

namespace Pis.Projekt.System
{
    public class AuthorizationParameters : IOperationFilter
    {
        public void Apply(OpenApiOperation operation,
            OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "user_id",
                In = ParameterLocation.Header,
                Style = ParameterStyle.Simple,
                Required = true
            });
            
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "password",
                In = ParameterLocation.Header,
                Style = ParameterStyle.Simple,
                Required = true
            });
        }
    }
}