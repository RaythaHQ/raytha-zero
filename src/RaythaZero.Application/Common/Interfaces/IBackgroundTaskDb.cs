using RaythaZero.Domain.Entities;

namespace RaythaZero.Application.Common.Interfaces;

public interface IBackgroundTaskDb
{
    BackgroundTask DequeueBackgroundTask();
}
