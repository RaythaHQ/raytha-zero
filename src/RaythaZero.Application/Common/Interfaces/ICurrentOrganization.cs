﻿using CSharpVitamins;
using RaythaZero.Application.AuthenticationSchemes;
using RaythaZero.Application.Common.Utils;

namespace RaythaZero.Application.Common.Interfaces;

public interface ICurrentOrganization
{
    bool InitialSetupComplete { get; }
    string OrganizationName { get; }
    string WebsiteUrl { get; }
    string TimeZone { get; }
    string DateFormat { get; }
    string SmtpDefaultFromAddress { get; }
    string SmtpDefaultFromName { get; }
    bool EmailAndPasswordIsEnabledForAdmins { get; }
    bool EmailAndPasswordIsEnabledForUsers { get; }
    string PathBase { get; }
    string RedirectWebsite { get; }

    OrganizationTimeZoneConverter TimeZoneConverter { get; }
    IEnumerable<AuthenticationSchemeDto> AuthenticationSchemes { get; }
}