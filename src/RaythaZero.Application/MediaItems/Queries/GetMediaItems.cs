using System.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Models;
using RaythaZero.Application.Common.Utils;
using RaythaZero.Domain.ValueObjects;

namespace RaythaZero.Application.MediaItems.Queries;

public class GetMediaItems
{
    public record Query : GetPagedEntitiesInputDto, IRequest<IQueryResponseDto<ListResultDto<MediaItemDto>>> 
    { 
        public override string OrderBy { get; init; } = $"CreationTime {SortOrder.DESCENDING}";
    }

    public class Handler : IRequestHandler<Query, IQueryResponseDto<ListResultDto<MediaItemDto>>>
    {
        private readonly IRaythaDbContext _db;
        public Handler(IRaythaDbContext db)
        {
            _db = db;
        }
        
        public async Task<IQueryResponseDto<ListResultDto<MediaItemDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _db.MediaItems.AsQueryable();
                           
            if (!string.IsNullOrEmpty(request.Search))
            {
                var searchQuery = request.Search.ToLower();
                query = query.Where(p => p.ObjectKey.ToLower().Contains(searchQuery));        
            }

            var total = await query.CountAsync();
            var items = query.ApplyPaginationInput(request).Select(MediaItemDto.GetProjection()).ToArray();

            return new QueryResponseDto<ListResultDto<MediaItemDto>>(new ListResultDto<MediaItemDto>(items, total));
        }
    }
}
