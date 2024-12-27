using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Utils;

namespace RaythaZero.Web.Areas.Shared.Models;

public class BasePageModel : PageModel
{
    public const string ErrorMessageKey = "ErrorMessage";
    public const string SuccessMessageKey = "SuccessMessage";
    public const string WarningMessageKey = "WarningMessage";
    
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
    
    public Dictionary<string, string> ValidationFailures { get; set; }
    
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
        
        string currentPage = RouteData.Values["page"]?.ToString();
        if (!CurrentOrganization.InitialSetupComplete && !string.IsNullOrEmpty(currentPage) && currentPage != "/Setup/Index")
        {
            context.HttpContext.Response.Redirect($"{CurrentOrganization.PathBase}/admin/setup");
            return;
        }
        
        await next();
        CheckIfErrorOrSuccessMessageExist();
    }
    
    protected void CheckIfErrorOrSuccessMessageExist()
    {
        if (TempData[ErrorMessageKey] != null)
        {
            ViewData[ErrorMessageKey] = TempData[ErrorMessageKey];
        }

        if (TempData[SuccessMessageKey] != null)
        {
            ViewData[SuccessMessageKey] = TempData[SuccessMessageKey];
        }
    }

    protected void SetErrorMessage(string message)
    {
        ViewData[ErrorMessageKey] = message;
        TempData[ErrorMessageKey] = message;
    }

    protected void SetErrorMessage(IEnumerable<ValidationFailure> errors, int statusCode = 303)
    {
        SetErrorMessage(string.Empty, errors, statusCode);
    }

    protected void SetErrorMessage(string message, IEnumerable<ValidationFailure> errors, int statusCode = 303)
    {
        var validationSummary = errors.FirstOrDefault(p => p.PropertyName == Constants.VALIDATION_SUMMARY);
        if (validationSummary != null)
        {
            SetErrorMessage(validationSummary.ErrorMessage);
        }
        else if (!string.IsNullOrEmpty(message))
        {
            SetErrorMessage(message);
        }
        else
        {
            SetErrorMessage("There was an error processing your request.");
        }

        ValidationFailures = errors?.ToDictionary(k => k.PropertyName, v => v.ErrorMessage);
        HttpContext.Response.StatusCode = statusCode;
    }

    protected void SetSuccessMessage(string message)
    {
        ViewData[SuccessMessageKey] = message;
        TempData[SuccessMessageKey] = message;
    }
    protected void SetWarningMessage(string message)
    {
        ViewData[WarningMessageKey] = message;
        TempData[WarningMessageKey] = message;
    }
    
    public string HasError(string propertyName)
    {
        return ValidationFailures != null && ValidationFailures.Any(p => p.Key == propertyName) ? "is-invalid" : string.Empty;
    }

    public string ErrorMessageFor(string propertyName)
    {
        return ValidationFailures?.FirstOrDefault(p => p.Key == propertyName).Value;
    }
}