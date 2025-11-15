using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Domain.Options;

[PublicAPI]
public class DatabaseOptions
{
    public const string Aso = nameof(Aso);
    public const string Cps = nameof(Cps);
    public static readonly DatabaseOptions Empty = new("", 0, "", "");

    public DatabaseOptions() { }

    [SetsRequiredMembers]
    public DatabaseOptions(
        string host,
        int port,
        string userId,
        string password,
        bool trustServerCertificate = true
    )
    {
        Host = host;
        Port = port;
        UserId = userId;
        Password = password;
        TrustServerCertificate = trustServerCertificate;
    }

    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string UserId { get; init; }
    public required string Password { get; init; }
    public bool TrustServerCertificate { get; init; } = true;
}
