using BegumYatch.Core.DTOs.Error;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace BegumYatch.API.Extensions
{
    public static class GlobalExceptionHandlerExtensions
    {
        public static void ConfigureGlobalExceptionHandler(
            this WebApplication app)
        {
            app.UseExceptionHandler(configure =>
            {
                configure.Run(async context =>
                {
                    #region when an error occured
                    var feature = context.Features.Get<IExceptionHandlerFeature>();
                    
                    // if feature is not null then any error has occured.
                    if (feature != null)
                    {
                        #region set default "status code" and "content type"
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "json";
                        #endregion

                        #region set error details
                        var exceptionMsg = feature.Error.Message;
                        ErrorDto errorDto;
                        
                        try
                        {
                            #region when excepted error occured
                            errorDto = JsonSerializer
                                .Deserialize<ErrorDto>(exceptionMsg);

                            context.Response.StatusCode = errorDto.StatusCode;
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            #region when unexcepted error occured
                            errorDto = new ErrorDto
                            {
                                StatusCode = context.Response.StatusCode,
                                ErrorCode = "ISE",
                                ErrorMessage = exceptionMsg
                            };
                            #endregion
                        }
                        #endregion

                        await context.Response.WriteAsJsonAsync(errorDto);
                    }
                    #endregion  
                });
            });
        }
    }
}
