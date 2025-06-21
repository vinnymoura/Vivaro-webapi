using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Shared.Models.Errors;

public class ConflictError : ProblemDetails
{
    public ConflictError(string message, HttpContext context)
    {
        Title = "Conflict";
        Detail = message;
        Instance = context.TraceIdentifier;
        Status = (int)HttpStatusCode.Conflict;
    }
}