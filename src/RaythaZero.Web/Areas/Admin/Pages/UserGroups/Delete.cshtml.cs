using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaythaZero.Application.UserGroups.Commands;
using RaythaZero.Domain.Entities;

namespace RaythaZero.Web.Areas.Admin.Pages.UserGroups;

[Authorize(Policy = BuiltInSystemPermission.MANAGE_USERS_PERMISSION)]
public class Delete : BaseAdminPageModel
{
    public async Task<IActionResult> OnPost(string id)
    {
        var response = await Mediator.Send(new DeleteUserGroup.Command { Id = id });
        if (response.Success)
        {
            SetSuccessMessage($"User group has been deleted.");
            return RedirectToPage("/UserGroups/Index"); 
        }
        else
        {
            SetErrorMessage("There was a problem deleting this user group", response.GetErrors());
            return RedirectToPage("/UserGroups/Edit", new { id = id }); 
        }
    }
}