using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaythaZero.Application.Roles.Commands;
using RaythaZero.Domain.Entities;

namespace RaythaZero.Web.Areas.Admin.Pages.Roles;

[Authorize(Policy = BuiltInSystemPermission.MANAGE_ADMINISTRATORS_PERMISSION)]
public class Delete : BaseAdminPageModel
{
    public async Task<IActionResult> OnPost(string id)
    {
        var response = await Mediator.Send(new DeleteRole.Command { Id = id });
        if (response.Success)
        {
            SetSuccessMessage($"Role has been deleted.");
            return RedirectToPage("/Roles/Index"); 
        }
        else
        {
            SetErrorMessage("There was a problem deleting this role", response.GetErrors());
            return RedirectToPage("/Roles/Edit", new { id }); 
        }
    }
}