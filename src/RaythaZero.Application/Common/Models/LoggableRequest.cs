using CSharpVitamins;
using MediatR;
using RaythaZero.Application.Common.Attributes;

namespace RaythaZero.Application.Common.Models;

public interface ILoggableRequest
{
}

public abstract record LoggableRequest<T> : IRequest<T>, ILoggableRequest
{
    public virtual string GetLogName()
    {
        return this.GetType()
            .FullName
            .Replace("RaythaZero.Application.", string.Empty)
            .Replace("+Command", string.Empty)
            .Replace("NavigationMenu", "Menu")
            .Replace("NavigationMenuItem", "MenuItem")
            .Replace("RaythaFunction", "Function");
    }
}

public interface ILoggableEntityRequest
{
}

public abstract record LoggableEntityRequest<T> : LoggableRequest<T>, ILoggableEntityRequest
{
    [ExcludePropertyFromOpenApiDocs]
    public virtual ShortGuid Id { get; init; }
}