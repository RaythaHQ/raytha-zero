using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RaythaZero.Application.Common.Security;
using RaythaZero.Web.Areas.Shared.Models;

namespace RaythaZero.Web.Areas.Admin.Pages;

[Area("Admin")]
[Authorize(Policy = RaythaClaimTypes.IsAdmin)]
public class BaseAdminPageModel : BasePageModel
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