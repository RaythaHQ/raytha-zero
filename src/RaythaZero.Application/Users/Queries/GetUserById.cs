using MediatR;
using Microsoft.EntityFrameworkCore;
using RaythaZero.Application.Common.Exceptions;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Models;

namespace RaythaZero.Application.Users.Queries;

public class GetUserById
{
    public record Query : GetEntityByIdInputDto, IRequest<IQueryResponseDto<UserDto>>
    {
    }

    public class Handler : IRequestHandler<Query, IQueryResponseDto<UserDto>>
    {
        private readonly IRaythaDbContext _db;
        public Handler(IRaythaDbContext db)
        {
            _db = db;
        }

        public async Task<IQueryResponseDto<UserDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = _db.Users
                .Include(p => p.UserGroups)
                .FirstOrDefault(p => p.Id == request.Id.Guid);

            if (entity == null)
                throw new NotFoundException("User", request.Id);

            return new QueryResponseDto<UserDto>(UserDto.GetProjection(entity));
        }
    }
}
