using Microsoft.AspNetCore.Mvc.Filters;

namespace DirectoryOfNaturalPersons.Middlewares;

public class ValidationActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var response = new BadHttpRequestException("One or more validation errors occurred");
            return;
        }

        await next();
    }
}