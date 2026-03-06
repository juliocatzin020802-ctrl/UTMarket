using System;

namespace UTMarket.Infrastructure.Settings;

public class DatabaseSettings
{
    public const string SectionName = "ConnectionStrings";

    private string _defaultConnection = string.Empty;
    public string DefaultConnection
    {
        get => _defaultConnection;
        set => _defaultConnection = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Database connection string cannot be empty.", nameof(value));
    }
}
