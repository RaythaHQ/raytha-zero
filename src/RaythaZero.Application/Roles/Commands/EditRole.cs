using CSharpVitamins;
using FluentValidation;
using MediatR;
using RaythaZero.Application.Common.Exceptions;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Models;
using RaythaZero.Domain.Entities;

namespace RaythaZero.Application.Roles.Commands;

public class EditRole
{
    public record Command : LoggableEntityRequest<CommandResponseDto<ShortGuid>>
    {
        public string Label { get; init; } = null!;
        public IEnumerable<string> SystemPermissions { get; init; } = null!;
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Label).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, CommandResponseDto<ShortGuid>>
    {
        private readonly IRaythaDbContext _db;
        public Handler(IRaythaDbContext db)
        {
            _db = db;
        }
        public async Task<CommandResponseDto<ShortGuid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var entity = _db.Roles.FirstOrDefault(p => p.Id == request.Id.Guid);
            if (entity == null)
                throw new NotFoundException("Role", request.Id);

            entity.Label = request.Label;

            //Don't modify super admin permissions ever
            if (entity.DeveloperName != BuiltInRole.SuperAdmin)
            {
                entity.SystemPermissions = BuiltInSystemPermission.From(request.SystemPermissions.ToArray());
            }

            await _db.SaveChangesAsync(cancellationToken);
            return new CommandResponseDto<ShortGuid>(entity.Id);
        }
    }
}
