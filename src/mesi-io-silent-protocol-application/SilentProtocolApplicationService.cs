using System;
using System.Linq;
using System.Threading.Tasks;
using Mesi.Io.SilentProtocol.Domain;
using Mesi.Io.SilentProtocol.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mesi.Io.SilentProtocol.Application
{
    /// <summary>
    /// Provides implementation for application layer use cases
    /// </summary>
    public class SilentProtocolApplicationService : IGetSilentProtocolEntriesPaged, IAddSilentProtocolEntry
    {
        private readonly ISilentProtocolEntryRepository _silentProtocolEntryRepository;
        private readonly ISilentProtocolEntryFactory _silentProtocolEntryFactory;
        private readonly SilentProtocolOptions _options;
        private readonly ILogger<SilentProtocolApplicationService> _logger;

        public SilentProtocolApplicationService(ISilentProtocolEntryRepository silentProtocolEntryRepository, ISilentProtocolEntryFactory silentProtocolEntryFactory, IOptionsSnapshot<SilentProtocolOptions> options, ILogger<SilentProtocolApplicationService> logger)
        {
            _silentProtocolEntryRepository = silentProtocolEntryRepository;
            _silentProtocolEntryFactory = silentProtocolEntryFactory;
            _options = options.Value;
            _logger = logger;
        }
        
        /// <inheritdoc />
        public async Task<GetSilentProtocolEntriesPagedResponse> GetPaged(int pageNumber)
        {
            var skipNEntries = (pageNumber - 1) * _options.NumberOfResultsPerPage;
            var results = (await _silentProtocolEntryRepository.GetSliced(skipNEntries, _options.NumberOfResultsPerPage + 1)).ToList();

            var hasMoreEntries = results.Count > _options.NumberOfResultsPerPage;
            return new(results.Take(_options.NumberOfResultsPerPage), hasMoreEntries);
        }

        /// <inheritdoc />
        public async Task<SilentProtocolEntry?> Add(AddSilentProtocolEntryRequest data)
        {
            try
            {
                var newEntry = _silentProtocolEntryFactory.Create(data.Suspect, data.Entry, data.TimeStamp, data.Reporter);
                await _silentProtocolEntryRepository.Save(newEntry);
                return newEntry;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Unable to add new entry to silent protocol. Reason: '{msg}'", ex.Message);
                return null;
            }
        }
    }
}