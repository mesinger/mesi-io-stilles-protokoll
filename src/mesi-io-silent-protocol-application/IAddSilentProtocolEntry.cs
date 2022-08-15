using System.Threading.Tasks;
using Mesi.Io.SilentProtocol.Domain;

namespace Mesi.Io.SilentProtocol.Application
{
    /// <summary>
    /// Adds a new <see cref="SilentProtocolEntry"/>
    /// </summary>
    public interface IAddSilentProtocolEntry
    {
        /// <summary>
        /// Adds a new <see cref="SilentProtocolEntry"/>
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<SilentProtocolEntry?> Add(AddSilentProtocolEntryRequest data);
    }

    public record AddSilentProtocolEntryRequest(string Suspect, string Entry, string TimeStamp, string Reporter);
}