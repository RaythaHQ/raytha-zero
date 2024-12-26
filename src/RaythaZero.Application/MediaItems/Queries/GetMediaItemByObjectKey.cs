using CSharpVitamins;
using MediatR;
using RaythaZero.Application.Common.Exceptions;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Models;

namespace RaythaZero.Application.MediaItems.Queries;

public class GetMediaItemByObjectKey
{
    public record Query : IRequest<IQueryResponseDto<MediaItemDto>>
    {
        public string ObjectKey { get; init; }
    }

    public class Handler : IRequestHandler<Query, IQueryResponseDto<MediaItemDto>>
    {
        private readonly IRaythaDbContext _db;
        public Handler(IRaythaDbContext db)
        {
            _db = db;
        } 
        
        public async Task<IQueryResponseDto<MediaItemDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = _db.MediaItems.FirstOrDefault(p => p.ObjectKey == request.ObjectKey);

            if (entity == null)
                throw new NotFoundException("Media item", request.ObjectKey);

            return new QueryResponseDto<MediaItemDto>(MediaItemDto.GetProjection(entity));
        }
    }
}