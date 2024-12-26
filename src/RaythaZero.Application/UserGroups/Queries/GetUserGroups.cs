﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Models;
using RaythaZero.Application.Common.Utils;
using RaythaZero.Domain.ValueObjects;

namespace RaythaZero.Application.UserGroups.Queries;

public class GetUserGroups
{
    public record Query : GetPagedEntitiesInputDto, IRequest<IQueryResponseDto<ListResultDto<UserGroupDto>>>
    {
        public override string OrderBy { get; init; } = $"Label {SortOrder.ASCENDING}";
    }

    public class Handler : IRequestHandler<Query, IQueryResponseDto<ListResultDto<UserGroupDto>>>
    {
        private readonly IRaythaDbContext _db;
        public Handler(IRaythaDbContext db)
        {
            _db = db;
        }

        public async Task<IQueryResponseDto<ListResultDto<UserGroupDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _db.UserGroups.AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                var searchQuery = request.Search.ToLower();
                query = query.Where(d =>
                    d.Label.ToLower().Contains(searchQuery) ||
                    d.DeveloperName.ToLower().Contains(searchQuery));
            }

            var total = await query.CountAsync();
            var items = query.ApplyPaginationInput(request).Select(UserGroupDto.GetProjection()).ToArray();

            return new QueryResponseDto<ListResultDto<UserGroupDto>>(new ListResultDto<UserGroupDto>(items, total));
        }
    }
}