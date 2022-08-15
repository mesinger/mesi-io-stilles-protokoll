using System;

namespace Mesi.Io.SilentProtocol.Domain
{
    public record SilentProtocolEntry(string Id, string Suspect, string Entry, string TimeStamp, DateTime CreatedAtUtc, string? Reporter);
}