using MediatR;
using Microsoft.EntityFrameworkCore;
using RaythaZero.Application.Common.Exceptions;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Models;
using RaythaZero.Application.Common.Utils;

namespace RaythaZero.Application.EmailTemplates.Queries;

public class GetEmailTemplateByName
{
    public record Query : IRequest<IQueryResponseDto<EmailTemplateDto>>
    {
        public string DeveloperName { get; init; }
    }

    public class Handler : IRequestHandler<Query, IQueryResponseDto<EmailTemplateDto>>
    {
        private readonly IRaythaDbContext _db;
        public Handler(IRaythaDbContext db)
        {
            _db = db;
        }

        public async Task<IQueryResponseDto<EmailTemplateDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = _db.EmailTemplates
                .FirstOrDefault(p => p.DeveloperName == request.DeveloperName.ToDeveloperName());

            if (entity == null)
                throw new NotFoundException("EmailTemplate", request.DeveloperName);

            return new QueryResponseDto<EmailTemplateDto>(EmailTemplateDto.GetProjection(entity));
        }
    }
}
