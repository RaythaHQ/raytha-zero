using CSharpVitamins;

namespace RaythaZero.Application.Common.Models;

public record GetEntityByIdInputDto
{
    public ShortGuid Id { get; init; }
}