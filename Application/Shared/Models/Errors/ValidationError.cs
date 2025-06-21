using System.Net;
using Application.Shared.Notifications;
using Microsoft.AspNetCore.Http;

namespace Application.Shared.Models.Errors;

public class ValidationError(Notification notification, HttpContext context)
{
    public string Title { get; set; } = "Bad request";
    public IDictionary<string, IList<string>>? Detail { get; set; } = notification.GetErrorMessages();
    public string Instance { get; set; } = context.TraceIdentifier;
    public int Status { get; set; } = (int)HttpStatusCode.BadRequest;
}