using Domain.Exceptions;
using Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using Web.Models;

namespace Web.Filters
{
    public class ApiExceptionFilter(ILogger<ApiExceptionFilter>logger) : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger=logger;
        public void OnException(ExceptionContext context)
        {
            var exception=context.Exception;
            int statusCode=400;//defaut
            ApiErrorResponse? errorResponse;
            switch (true)
            {
                case { } when exception is DuplicateEntityException:
                    {
                        errorResponse = new ApiErrorResponse
                        {
                            Code=11,//duplicate
                            Message=exception.Message,
                            Description=exception.ToText()
                        };
                        break;
                    }
                case { } when exception is EntityNotFound:
                    {
                        statusCode = 400;
                        errorResponse = new ApiErrorResponse
                        {
                            Code = 12,//not found entity
                            Message = exception.Message,
                            Description = exception.ToText()
                        };
                        break;
                    }
                default:
                    {
                        errorResponse=new ApiErrorResponse { Code=99,Message=exception.Message, Description=exception.ToText() };//another errors
                        break;
                    }
            }
            logger.LogError($"Api {context.HttpContext.Request.Path} finished with code: {statusCode} => Error:{JsonSerializer.Serialize(errorResponse)}");
            context.Result = new JsonResult(new { errorResponse }) { StatusCode=statusCode};
        }
    }
}
