using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopDomain.Models;

public class UserCheckFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.ActionArguments["user"] as User;

        Console.WriteLine($"User: {user?.Login}, Id: {user?.Id}");

        if (user?.Id == 1 && user?.Login == "admin")
        {
            Console.WriteLine("Access granted");
            return;
        }

        Console.WriteLine("Access denied");

        context.Result = new JsonResult(new { message = "No authorization" })
        {
            StatusCode = 400
        };
    }
}