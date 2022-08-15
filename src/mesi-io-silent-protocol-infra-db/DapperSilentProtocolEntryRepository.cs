using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mesi.Io.SilentProtocol.Domain;

namespace Mesi.Io.SilentProtocol.Infrastructure.Db;

public class SilentProtocolEntryDataModel
{
    public string id { get; set; } = null!;
    public string suspect { get; set; } = null!;
    public string entry { get; set; } = null!;
    public string time_stamp { get; set; } = null!;
    public DateTime created_at_utc { get; set; }
    public string reporter { get; set; }
}

public class DapperSilentProtocolEntryRepository : ISilentProtocolEntryRepository
{
    private readonly IDbConnection _dbConnection;

    public DapperSilentProtocolEntryRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    
    public async Task<IEnumerable<SilentProtocolEntry>> GetSliced(int skip, int take)
    {
        var entries = await _dbConnection.QueryAsync<SilentProtocolEntryDataModel>("select * from t_silent_protocol_entries order by created_at_utc desc");
        return entries.Skip(skip).Take(take).Select(data => new SilentProtocolEntry(data.id, data.suspect, data.entry, data.time_stamp, data.created_at_utc, data.reporter));
    }

    public async Task Save(SilentProtocolEntry entry)
    {
        await _dbConnection.ExecuteAsync(
            "insert into t_silent_protocol_entries values (@id, @suspect, @entry, @timeStamp, @createdAt, @reporter)", new
            {
                id = entry.Id,
                suspect = entry.Suspect,
                entry = entry.Entry,
                timeStamp = entry.TimeStamp,
                createdAt = entry.CreatedAtUtc,
                reporter = entry.Reporter,
            });
    }

    public async Task Update(SilentProtocolEntry entry)
    {
        await _dbConnection.ExecuteAsync(
            "update t_silent_protocol_entries set suspect = @suspect, entry = @entry, time_stamp = @timeStamp where id = @id",
            new
            {
                id = entry.Id,
                suspect = entry.Suspect,
                entry = entry.Entry,
                timeStamp = entry.TimeStamp,
            });
    }

    public async Task<SilentProtocolEntry> GetById(string id)
    {
        var entry = await _dbConnection.QuerySingleOrDefaultAsync<SilentProtocolEntryDataModel>(
            "select * from t_silent_protocol_entries where id = @id", new { id });

        return entry is not null
            ? new SilentProtocolEntry(entry.id, entry.suspect, entry.entry, entry.time_stamp, entry.created_at_utc, entry.reporter)
            : null;
    }
}