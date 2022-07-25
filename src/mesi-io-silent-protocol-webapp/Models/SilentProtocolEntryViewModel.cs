using Mesi.Io.SilentProtocol.Domain;

namespace Mesi.Io.SilentProtocol.WebApp.Models
{
    public record SilentProtocolEntryViewModel(string Id, string Suspect, string Entry, string TimeStamp);

    internal static partial class ViewModelMapping
    {
        public static SilentProtocolEntryViewModel ToViewModel(this SilentProtocolEntry entry) =>
            new(entry.Id, entry.Suspect, entry.Entry, entry.TimeStamp);
    }
}