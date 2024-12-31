using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaythaZero.Application.Admins.Commands;
using RaythaZero.Domain.Entities;

namespace RaythaZero.Web.Areas.Admin.Pages.Admins;

[Authorize(Policy = BuiltInSystemPermission.MANAGE_ADMINISTRATORS_PERMISSION)]
public class Restore : BaseAdminPageModel
{
    public async Task<IActionResult> OnPost(string id)
    {
        var response = await Mediator.Send(new SetIsActive.Command { Id = id, IsActive = true });
        if (response.Success)
        {
            SetSuccessMessage($"Account has been restored.");
        }
        else
        {
            SetErrorMessage(response.Error);
        }

        return RedirectToPage("/Admins/Edit", new { id }); 
    }
}