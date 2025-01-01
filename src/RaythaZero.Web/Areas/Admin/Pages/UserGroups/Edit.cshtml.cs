using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaythaZero.Application.UserGroups.Commands;
using RaythaZero.Application.UserGroups.Queries;
using RaythaZero.Domain.Entities;

namespace RaythaZero.Web.Areas.Admin.Pages.UserGroups;

[Authorize(Policy = BuiltInSystemPermission.MANAGE_USERS_PERMISSION)]
public class Edit : BaseAdminPageModel
{
    [BindProperty]
    public FormModel Form { get; set; }
    public async Task<IActionResult> OnGet(string id)
    {
        var response = await Mediator.Send(new GetUserGroupById.Query { Id = id });

        Form = new FormModel 
        {
            Id = response.Result.Id,
            Label = response.Result.Label,
            DeveloperName = response.Result.DeveloperName
        };
        return Page();
    }

    public async Task<IActionResult> OnPost(string id)
    {
        var input = new EditUserGroup.Command
        {
            Id = id,
            Label = Form.Label
        };

        var response = await Mediator.Send(input);

        if (response.Success)
        {
            SetSuccessMessage($"{Form.Label} was updated successfully.");
            return RedirectToPage("/UserGroups/Edit", new { id });
        }
        {
            SetErrorMessage("There was an error attempting to update this user group. See the error below.", response.GetErrors());
            return Page();
        } 
    }

    public record FormModel
    {
        public string Id { get; set; }

        [Display(Name = "Label")]
        public string Label { get; set; }

        [Display(Name = "Developer name")]
        public string DeveloperName { get; set; } 
    }
}