using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaythaZero.Domain.Entities;
using RaythaZero.Domain.ValueObjects;
using RaythaZero.Web.Areas.Shared.Models;

namespace RaythaZero.Web.Areas.Admin.Pages.Users;

[Authorize(Policy = BuiltInSystemPermission.MANAGE_USERS_PERMISSION)]
public class Index : BaseAdminPageModel, IHasListView<Index.AuthenticationSchemesListViewModel>
{
    public ListViewModel<AuthenticationSchemesListViewModel> ListView { get; set; }
    public async Task<IActionResult> OnGet(
        string search = "", string orderBy = $"Label {SortOrder.ASCENDING}", int pageNumber = 1, int pageSize = 50)
    {
        return Page();
    }

    public record AuthenticationSchemesListViewModel
    {
        
    }
}
