﻿namespace RaythaZero.Application.Dashboard;

public record DashboardDto
{
    public long FileStorageSize { get; init; }
    public int TotalUsers { get; init; }
    public decimal DbSize { get; init; }
}
