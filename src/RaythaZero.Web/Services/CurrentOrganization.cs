using CSharpVitamins;
using MediatR;
using RaythaZero.Application.AuthenticationSchemes;
using RaythaZero.Application.AuthenticationSchemes.Queries;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Utils;
using RaythaZero.Application.OrganizationSettings;
using RaythaZero.Application.OrganizationSettings.Queries;
using RaythaZero.Domain.Entities;
using RaythaZero.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Raytha.Web.Services;

public class CurrentOrganization : ICurrentOrganization
{
    private OrganizationSettingsDto _organizationSettings;
    private IEnumerable<AuthenticationSchemeDto> _authenticationSchemes;

    private readonly ISender Mediator;
    private readonly ICurrentOrganizationConfiguration Configuration;

    public CurrentOrganization(ISender mediator, ICurrentOrganizationConfiguration configuration)
    {
        Mediator = mediator;
        Configuration = configuration;
    }

    private OrganizationSettingsDto OrganizationSettings
    {
        get
        {
            if (_organizationSettings == null)
            {
                var response = Mediator.Send(new GetOrganizationSettings.Query()).GetAwaiter().GetResult();
                _organizationSettings = response.Result;
            }
            return _organizationSettings;
        }
    }

    public IEnumerable<AuthenticationSchemeDto> AuthenticationSchemes 
    { 
        get
        {
            if (_authenticationSchemes == null)
            {
                var response = Mediator.Send(new GetAuthenticationSchemes.Query()).GetAwaiter().GetResult();
                _authenticationSchemes = response.Result.Items;
            }
            return _authenticationSchemes;
        }
    }

    public bool EmailAndPasswordIsEnabledForAdmins => AuthenticationSchemes.Any(p => p.IsEnabledForAdmins && p.AuthenticationSchemeType.DeveloperName == AuthenticationSchemeType.EmailAndPassword);
    public bool EmailAndPasswordIsEnabledForUsers => AuthenticationSchemes.Any(p => p.IsEnabledForUsers && p.AuthenticationSchemeType.DeveloperName == AuthenticationSchemeType.EmailAndPassword);
    
    public bool InitialSetupComplete => OrganizationSettings != null;
    public string OrganizationName => OrganizationSettings.OrganizationName;
    public string WebsiteUrl => OrganizationSettings.WebsiteUrl;
    public string TimeZone => OrganizationSettings.TimeZone;
    public string SmtpDefaultFromAddress => OrganizationSettings.SmtpDefaultFromAddress;
    public string SmtpDefaultFromName => OrganizationSettings.SmtpDefaultFromName;
    public string DateFormat => OrganizationSettings.DateFormat;

    public OrganizationTimeZoneConverter TimeZoneConverter => OrganizationTimeZoneConverter.From(TimeZone, DateFormat);

    public string PathBase => Configuration.PathBase;
    public string RedirectWebsite => Configuration.RedirectWebsite;
}
