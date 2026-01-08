namespace Domain.Models;

public readonly record struct Tick(
long TimestampNs,
    string Symbol,
    double Price,
    double Volume
);