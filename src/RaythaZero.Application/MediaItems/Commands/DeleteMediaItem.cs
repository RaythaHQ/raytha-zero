using CSharpVitamins;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RaythaZero.Application.Common.Exceptions;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Models;

namespace RaythaZero.Application.MediaItems.Commands;

public class DeleteMediaItem
{
    public record Command : LoggableEntityRequest<CommandResponseDto<ShortGuid>>
    {
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator(IRaythaDbContext db)
        {
            RuleFor(x => x).Custom((request, _) =>
            {
                if (!db.MediaItems.Any(mi => mi.Id == request.Id.Guid))
                    throw new NotFoundException("Media Item", request.Id);
            });
        }
    }

    public class Handler : IRequestHandler<Command, CommandResponseDto<ShortGuid>>
    {
        private readonly IRaythaDbContext _db;
        private readonly IFileStorageProvider _fileStorageProvider;

        public Handler(IRaythaDbContext db, IFileStorageProvider fileStorageProvider)
        {
            _db = db;
            _fileStorageProvider = fileStorageProvider;
        }

        public async Task<CommandResponseDto<ShortGuid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var mediaItem = await _db.MediaItems
                .FirstAsync(mi => mi.Id == request.Id.Guid, cancellationToken);

            await _fileStorageProvider.DeleteAsync(mediaItem.ObjectKey);

            _db.MediaItems.Remove(mediaItem);

            await _db.SaveChangesAsync(cancellationToken);

            return new CommandResponseDto<ShortGuid>(mediaItem.Id);
        }
    }
}