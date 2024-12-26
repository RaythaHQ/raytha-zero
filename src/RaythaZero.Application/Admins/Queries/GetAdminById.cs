using MediatR;
using Microsoft.EntityFrameworkCore;
using RaythaZero.Application.Common.Exceptions;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Models;

namespace RaythaZero.Application.Admins.Queries;

public class GetAdminById
{
    public record Query : GetEntityByIdInputDto, IRequest<IQueryResponseDto<AdminDto>> 
    {
    }

    public class Handler : IRequestHandler<Query, IQueryResponseDto<AdminDto>>
    {
        private readonly IRaythaDbContext _db;
        public Handler(IRaythaDbContext db)
        {
            _db = db;
        }
        
        public async Task<IQueryResponseDto<AdminDto>> Handle(Query request, CancellationToken cancellationToken)
        {                   
            var entity = _db.Users
                .Include(p => p.Roles)
                .FirstOrDefault(p => p.Id == request.Id.Guid);

            if (entity == null)
                throw new NotFoundException("Admin", request.Id);

            return new QueryResponseDto<AdminDto>(AdminDto.GetProjection(entity));
        }
    }
}
