using System;

namespace Mesi.Io.SilentProtocol.Infrastructure.Db;

internal class AuditLogDataModel
{
    public string id { get; set; }
    public string user_id { get; set; }
    public string user_name { get; set; }
    public string type { get; set; }
    public string human_readable_info { get; set; }

    public DateTime time_stamp_utc { get; set; }
}