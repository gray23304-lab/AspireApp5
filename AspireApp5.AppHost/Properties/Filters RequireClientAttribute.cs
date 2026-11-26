using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ParcelTrack.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RequireClientAttribute : ActionFilterAttribute
{
    private const string HeaderName = "X-Client";
    private const string ActionExecutedKey = "RequireClient_ActionExecuted";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var clientHeader = context.HttpContext.Request.Headers[HeaderName].ToString().Trim();
        var actionName = context.ActionDescriptor.RouteValues.TryGetValue("action", out var act) && !string.IsNullOrWhiteSpace(act)
            ? act
            : context.ActionDescriptor.DisplayName ?? "Action";

        if (string.IsNullOrWhiteSpace(clientHeader))
        {
            context.Result = new BadRequestObjectResult(new
            {
                message = "Say which program is calling: add an X-Client header"
            });
            return;
        }

        context.HttpContext.Items[ActionExecutedKey] = true;
        Console.WriteLine($"X-Client: {clientHeader} -> {actionName}");
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.HttpContext.Items.ContainsKey(ActionExecutedKey))
        {
            var actionName = context.ActionDescriptor.RouteValues.TryGetValue("action", out var act) && !string.IsNullOrWhiteSpace(act)
                ? act
                : context.ActionDescriptor.DisplayName ?? "Action";

            Console.WriteLine($"{actionName} finished");
        }
    }
}
