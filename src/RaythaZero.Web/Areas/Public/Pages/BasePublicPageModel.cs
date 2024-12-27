using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RaythaZero.Web.Areas.Shared.Models;

namespace RaythaZero.Web.Areas.Public.Pages;

[Area("Public")]
public class BasePublicPageModel : BasePageModel
{
    public override async Task OnPageHandlerExecutionAsync(
        PageHandlerExecutingContext context,
        PageHandlerExecutionDelegate next)
    {
        // Shared logic before handler execution
        await base.OnPageHandlerExecutionAsync(context, next);
        
        await next();
    }
}