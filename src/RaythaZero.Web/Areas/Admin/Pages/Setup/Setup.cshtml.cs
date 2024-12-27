using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Utils;
using RaythaZero.Web.Areas.Shared.Models;

namespace RaythaZero.Web.Areas.Admin.Pages.Setup;

public class Setup : PageModel
{
    private ISender _mediator;
    private ICurrentOrganization _currentOrganization;
    private IEmailerConfiguration _emailerConfiguration;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    protected ICurrentOrganization CurrentOrganization => _currentOrganization ??= HttpContext.RequestServices.GetRequiredService<ICurrentOrganization>();
    protected IEmailerConfiguration EmailerConfiguration => _emailerConfiguration ??= HttpContext.RequestServices.GetRequiredService<IEmailerConfiguration>();

    [BindProperty] 
    public FormModel Form { get; set; }
    
    //helpers
    public bool MissingSmtpEnvironmentVariables { get; set; }
    public IDictionary<string, string> TimeZones
    {
        get { return DateTimeExtensions.GetTimeZoneDisplayNames(); }
    } 
    
    public async Task<IActionResult> OnGet()
    {
        if (CurrentOrganization.InitialSetupComplete)
            return RedirectToPage("/");

        MissingSmtpEnvironmentVariables = EmailerConfiguration.IsMissingSmtpEnvVars();
        Form = new FormModel();
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var formPost = Form;
        return Page();
    }

    public class FormModel : FormSubmit_ViewModel
    {
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        
        [Display(Name = "Email address")]
        public string SuperAdminEmailAddress { get; set; }

        [Display(Name = "Password")]
        public string SuperAdminPassword { get; set; }

        [Display(Name = "Organization name")]
        public string OrganizationName { get; set; }
        
        [Display(Name = "Website url")]
        public string WebsiteUrl { get; set; }

        [Display(Name = "Time zone")]
        public string TimeZone { get; set; }
        
        [Display(Name = "Default reply-to email address")]
        public string SmtpDefaultFromAddress { get; set; }

        [Display(Name = "Default reply-to name")]
        public string SmtpDefaultFromName { get; set; }    

        [Display(Name = "Host")]
        public string SmtpHost { get; set; }

        [Display(Name = "Port")]
        public int? SmtpPort { get; set; }

        [Display(Name = "Username")]
        public string SmtpUsername { get; set; }

        [Display(Name = "Password")]
        public string SmtpPassword { get; set; }
    }
}