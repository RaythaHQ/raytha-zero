using MediatR;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Models;

namespace RaythaZero.Application.OrganizationSettings.Queries;

public class GetOrganizationSettings
{
    public record Query : IRequest<IQueryResponseDto<OrganizationSettingsDto>>
    {
    }

    public class Handler : IRequestHandler<Query, IQueryResponseDto<OrganizationSettingsDto>>
    {
        private readonly IRaythaDbContext _db;
        public Handler(IRaythaDbContext db)
        {
            _db = db;
        }

        public async Task<IQueryResponseDto<OrganizationSettingsDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var settings = _db.OrganizationSettings.FirstOrDefault();

            return new QueryResponseDto<OrganizationSettingsDto>(OrganizationSettingsDto.GetProjection(settings));
        }
    }
}
