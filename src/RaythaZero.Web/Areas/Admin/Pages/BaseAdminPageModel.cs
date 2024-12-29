using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaythaZero.Application.Common.Security;
using RaythaZero.Web.Areas.Shared.Models;

namespace RaythaZero.Web.Areas.Admin.Pages;

[Area("Admin")]
[Authorize(Policy = RaythaClaimTypes.IsAdmin)]
public class BaseAdminPageModel : BasePageModel
{
}