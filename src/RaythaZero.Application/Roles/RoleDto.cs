using CSharpVitamins;
using RaythaZero.Application.Common.Models;
using RaythaZero.Domain.Entities;
using System.Linq.Expressions;

namespace RaythaZero.Application.Roles;

public record RoleDto : BaseAuditableEntityDto
{
    public string Label { get; init; } = string.Empty;
    public string DeveloperName { get; init; } = string.Empty;
    public IEnumerable<string> SystemPermissions { get; init; } = null!;
    public Dictionary<ShortGuid, IEnumerable<string>> ContentTypePermissions { get; init; } = null!;
    public Dictionary<string, IEnumerable<string>> ContentTypePermissionsFriendlyNames { get; init; } = null!;

    public static Expression<Func<Role, RoleDto>> GetProjection()
    {
        return entity => GetProjection(entity);
    }
    public static RoleDto GetProjection(Role entity)
    {
        return new RoleDto
        {
            Id = entity.Id,
            Label = entity.Label,
            DeveloperName = entity.DeveloperName,
            CreatorUserId = entity.CreatorUserId,
            CreationTime = entity.CreationTime,
            LastModificationTime = entity.LastModificationTime,
            LastModifierUserId = entity.LastModifierUserId,
            SystemPermissions = BuiltInSystemPermission.From(entity.SystemPermissions).Select(p => p.DeveloperName)
        };
    }
}