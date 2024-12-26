using MediatR;
using RaythaZero.Application.Common.Exceptions;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Models;

namespace RaythaZero.Application.Roles.Queries;

public class GetRoleById
{
    public record Query : GetEntityByIdInputDto, IRequest<IQueryResponseDto<RoleDto>>
    {
    }

    public class Handler : IRequestHandler<Query, IQueryResponseDto<RoleDto>>
    {
        private readonly IRaythaDbContext _db;
        public Handler(IRaythaDbContext db)
        {
            _db = db;
        }

        public async Task<IQueryResponseDto<RoleDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = _db.Roles
                .FirstOrDefault(p => p.Id == request.Id.Guid);

            if (entity == null)
                throw new NotFoundException("Role", request.Id);

            return new QueryResponseDto<RoleDto>(RoleDto.GetProjection(entity));
        }
    }
}
