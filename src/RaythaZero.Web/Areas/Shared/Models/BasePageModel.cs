using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Utils;

namespace RaythaZero.Web.Areas.Shared.Models;

public class BasePageModel : PageModel
{
    private ISender _mediator;
    private ICurrentOrganization _currentOrganization;
    private ICurrentUser _currentUser;
    private IFileStorageProviderSettings _fileStorageSettings;
    private IFileStorageProvider _fileStorageProvider;
    private IEmailer _emailer;
    private IEmailerConfiguration _emailerConfiguration;  
    
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    protected ICurrentOrganization CurrentOrganization => _currentOrganization ??= HttpContext.RequestServices.GetRequiredService<ICurrentOrganization>(); 
    protected ICurrentUser CurrentUser => _currentUser ??= HttpContext.RequestServices.GetRequiredService<ICurrentUser>();
    protected IFileStorageProvider FileStorageProvider => _fileStorageProvider ??= HttpContext.RequestServices.GetRequiredService<IFileStorageProvider>();
    protected IFileStorageProviderSettings FileStorageProviderSettings => _fileStorageSettings ??= HttpContext.RequestServices.GetRequiredService<IFileStorageProviderSettings>();
    protected IEmailer Emailer => _emailer ??= HttpContext.RequestServices.GetRequiredService<IEmailer>();
    protected IEmailerConfiguration EmailerConfiguration => _emailerConfiguration ??= HttpContext.RequestServices.GetRequiredService<IEmailerConfiguration>();
    
    public override async Task OnPageHandlerExecutionAsync(
        PageHandlerExecutingContext context,
        PageHandlerExecutionDelegate next)
    {
        // Shared logic before handler execution
        await base.OnPageHandlerExecutionAsync(context, next);
        if (CurrentOrganization.RedirectWebsite.IsValidUriFormat())
        {
            context.HttpContext.Response.Redirect(CurrentOrganization.RedirectWebsite);
            return;
        }
        if (!CurrentOrganization.InitialSetupComplete)
        {
            context.HttpContext.Response.Redirect($"/admin/setup");
            return;
        }
        
        await next();
    }
}