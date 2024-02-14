using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Service.Middlewares
{
    public class ResponseFormatterMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseFormatterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next.Invoke(context);

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(new ResponseModel("You are Status 401 Unauthorized")));
            }
            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(new ResponseModel("You are Status 403 Forbidden")));
            }         
        }
    }
    public class ResponseModel
    {
        public ResponseModel(string message)
        {
            this.Message = message;
        }

        public string Message { get; set; }
    }
}
