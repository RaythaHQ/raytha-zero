using MediatR;
using RaythaZero.Application.Common.Exceptions;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Models;

namespace RaythaZero.Application.AuthenticationSchemes.Queries;

public class GetAuthenticationSchemeById
{
    public record Query : GetEntityByIdInputDto, IRequest<IQueryResponseDto<AuthenticationSchemeDto>> 
    {
    }

    public class Handler : IRequestHandler<Query, IQueryResponseDto<AuthenticationSchemeDto>>
    {
        private readonly IRaythaDbContext _db;
        public Handler(IRaythaDbContext db)
        {
            _db = db;
        }
        public async Task<IQueryResponseDto<AuthenticationSchemeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = _db.AuthenticationSchemes.FirstOrDefault(p => p.Id == request.Id.Guid);

            if (entity == null)
                throw new NotFoundException("Authentication Scheme", request.Id);

            return new QueryResponseDto<AuthenticationSchemeDto>(AuthenticationSchemeDto.GetProjection(entity));
        }
    }
}