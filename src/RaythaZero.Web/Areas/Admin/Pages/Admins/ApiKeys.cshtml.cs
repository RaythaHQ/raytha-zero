using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaythaZero.Application.Admins.Commands;
using RaythaZero.Application.Admins.Queries;
using RaythaZero.Domain.Entities;
using RaythaZero.Domain.ValueObjects;
using RaythaZero.Web.Areas.Shared.Models;

namespace RaythaZero.Web.Areas.Admin.Pages.Admins;

[Authorize(Policy = BuiltInSystemPermission.MANAGE_ADMINISTRATORS_PERMISSION)]
public class ApiKeys : BaseAdminPageModel, IHasListView<ApiKeys.ApiKeysListItemViewModel>, ISubActionViewModel
{
    public string CreatedApiKey { get; set; }
    public string Id { get; set; }
    public bool IsActive { get; set; }
    
    public ListViewModel<ApiKeysListItemViewModel> ListView { get; set; }
    
    public async Task<IActionResult> OnGet(string id, string orderBy = $"CreationTime {SortOrder.ASCENDING}", int pageNumber = 1, int pageSize = 50)
    {
        var input = new GetApiKeysForAdmin.Query
        {
            UserId = id,
            OrderBy = orderBy,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var response = await Mediator.Send(input);

        var items = response.Result.Items.Select(p => new ApiKeysListItemViewModel
        {
            Id = p.Id,
            CreationTime = CurrentOrganization.TimeZoneConverter.UtcToTimeZoneAsDateTimeFormat(p.CreationTime)
        });

        ListView = new ListViewModel<ApiKeysListItemViewModel>(items, response.Result.TotalCount);

        if (TempData["ApiKey"] != null)
        {
            CreatedApiKey = TempData["ApiKey"].ToString();
        }
        
        var admin = await Mediator.Send(new GetAdminById.Query { Id = id });
        Id = id;
        IsActive = admin.Result.IsActive;
        return Page();
    }

    public async Task<IActionResult> OnPost(string id)
    {
        var response = await Mediator.Send(new CreateApiKey.Command { UserId = id });
        if (response.Success)
        {
            TempData["ApiKey"] = response.Result;
        }
        else
        {
            SetErrorMessage(response.Error, response.GetErrors());
        }
        return RedirectToPage("ApiKeys", new { id }); 
    }
    
    public class ApiKeysListItemViewModel
    {
        public string Id { get; init; }

        [Display(Name = "Created at")]
        public string CreationTime { get; init; }
    }
}