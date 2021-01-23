using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mesi.Io.SilentProtocol.Domain;
using Microsoft.EntityFrameworkCore;

namespace Mesi.Io.SilentProtocol.Infrastructure.Db
{
    /// <summary>
    /// A rdbs implementation for <see cref="ISilentProtocolEntryRepository"/>
    /// </summary>
    public class SilentProtocolEntryRepository : ISilentProtocolEntryRepository
    {
        private readonly SilentProtocolDbContext _dbContext;

        public SilentProtocolEntryRepository(SilentProtocolDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        /// <inheritdoc />
        public async Task<IEnumerable<SilentProtocolEntry>> GetSliced(int skip, int take)
        {
            var entries = await _dbContext.Entries.OrderByDescending(entry => entry.CreatedAtUtc).ToListAsync();
            return entries.Skip(skip).Take(take);
        }

        /// <inheritdoc />
        public async Task Save(SilentProtocolEntry entry)
        {
            await _dbContext.Entries.AddAsync(entry);
            await _dbContext.SaveChangesAsync();
        }
    }
}