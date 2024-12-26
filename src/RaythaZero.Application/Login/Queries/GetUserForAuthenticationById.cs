using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RaythaZero.Application.Common.Exceptions;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Models;

namespace RaythaZero.Application.Login.Queries;

public class GetUserForAuthenticationById
{
    public record Query : GetEntityByIdInputDto, IRequest<IQueryResponseDto<LoginDto>>
    {
    }

    public class Handler : IRequestHandler<Query, IQueryResponseDto<LoginDto>>
    {
        private readonly IRaythaDbContext _db;
        public Handler(IRaythaDbContext db)
        {
            _db = db;
        }
        
        public async Task<IQueryResponseDto<LoginDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = _db.Users
                .Include(p => p.Roles)
                .Include(p => p.UserGroups)
                .Include(p => p.AuthenticationScheme)
                .FirstOrDefault(p => p.Id == request.Id.Guid);

            if (entity == null)
                throw new NotFoundException("User", request.Id);

            return new QueryResponseDto<LoginDto>(LoginDto.GetProjection(entity));
        }
    }
}