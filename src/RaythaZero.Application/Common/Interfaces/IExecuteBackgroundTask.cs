using System.Text.Json;

namespace RaythaZero.Application.Common.Interfaces;
public interface IExecuteBackgroundTask
{
    Task Execute(Guid jobId, JsonElement args, CancellationToken cancellationToken);
}
