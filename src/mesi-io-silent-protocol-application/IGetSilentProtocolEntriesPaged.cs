using System.Collections.Generic;
using System.Threading.Tasks;
using Mesi.Io.SilentProtocol.Domain;

namespace Mesi.Io.SilentProtocol.Application
{
    /// <summary>
    /// Retrieves <see cref="SilentProtocolEntry"/>s for a given page
    /// </summary>
    public interface IGetSilentProtocolEntriesPaged
    {
        /// <summary>
        /// Retrieves <see cref="SilentProtocolEntry"/>s for a given page
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        Task<GetSilentProtocolEntriesPagedResponse> GetPaged(int pageNumber);
    }
    
    public record GetSilentProtocolEntriesPagedResponse(IEnumerable<SilentProtocolEntry> Entries, bool HasMoreEntries);
}