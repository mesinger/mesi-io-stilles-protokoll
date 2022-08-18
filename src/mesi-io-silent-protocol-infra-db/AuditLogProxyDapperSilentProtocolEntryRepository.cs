using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Mesi.Io.SilentProtocol.Application;
using Mesi.Io.SilentProtocol.Domain;

namespace Mesi.Io.SilentProtocol.Infrastructure.Db;

public class AuditLogProxyDapperSilentProtocolEntryRepository : ISilentProtocolEntryRepository
{
    private readonly IDbConnection _connection;
    private readonly IRequestContext _requestContext;
    private readonly ISilentProtocolEntryRepository _subject;
    
    public AuditLogProxyDapperSilentProtocolEntryRepository(IDbConnection connection, IRequestContext requestContext)
    {
        _connection = connection;
        _requestContext = requestContext;
        _subject = new DapperSilentProtocolEntryRepository(connection);
    }
    
    public Task<IEnumerable<SilentProtocolEntry>> GetSliced(int skip, int take)
    {
        return _subject.GetSliced(skip, take);
    }

    public async Task Save(SilentProtocolEntry entry)
    {
        await LogSaving(entry);
        await _subject.Save(entry);
    }

    public async Task Update(SilentProtocolEntry entry)
    {
        await LogUpdate(entry);
        await _subject.Update(entry);
    }

    public Task<SilentProtocolEntry> GetById(string id)
    {
        return _subject.GetById(id);
    }

    private async Task LogSaving(SilentProtocolEntry entry)
    {
        var user = _requestContext.User();
        await _connection.ExecuteAsync(
            "insert into audit_log values (@id, @userId, @userName, @type, @info, @timestamp)", new
            {
                id = Guid.NewGuid().ToString(),
                userId = user.Id,
                userName = user.Name,
                type = "save",
                info = entry.ToString(),
                timestamp = DateTime.UtcNow,
            });
    }
    
    private async Task LogUpdate(SilentProtocolEntry entry)
    {
        var user = _requestContext.User();
        await _connection.ExecuteAsync(
            "insert into audit_log values (@id, @userId, @userName, @type, @info, @timestamp)", new
            {
                id = Guid.NewGuid().ToString(),
                userId = user.Id,
                userName = user.Name,
                type = "update",
                info = entry.ToString(),
                timestamp = DateTime.UtcNow,
            });
    }
}