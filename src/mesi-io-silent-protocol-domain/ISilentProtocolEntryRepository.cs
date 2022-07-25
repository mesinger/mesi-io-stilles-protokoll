using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mesi.Io.SilentProtocol.Domain
{
    /// <summary>
    /// Data access to <see cref="SilentProtocolEntry"/>s 
    /// </summary>
    public interface ISilentProtocolEntryRepository
    {
        /// <summary>
        /// Skips <paramref name="skip"/> entries and takes <paramref name="take"/> <see cref="SilentProtocolEntry"/>s
        /// </summary>
        /// <param name="skip">Number of entries that are skipped</param>
        /// <param name="take">Number of entries that shall be returned</param>
        /// <returns></returns>
        Task<IEnumerable<SilentProtocolEntry>> GetSliced(int skip, int take);

        /// <summary>
        /// Persists a new <see cref="SilentProtocolEntry"/>
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        Task Save(SilentProtocolEntry entry);

        /// <summary>
        /// Updates an existing <see cref="SilentProtocolEntry"/>
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        Task Update(SilentProtocolEntry entry);

        /// <summary>
        /// Returns an entry identified by <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SilentProtocolEntry?> GetById(string id);
    }
}