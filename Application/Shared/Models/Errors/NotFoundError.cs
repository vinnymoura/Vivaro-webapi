using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Shared.Models.Errors;

public class NotFoundError : ProblemDetails
{
    public NotFoundError(string message, HttpContext context)
    {
        Title = "Not Found";
        Detail = message;
        Instance = context.TraceIdentifier;
        Status = (int)HttpStatusCode.NotFound;
    }
}