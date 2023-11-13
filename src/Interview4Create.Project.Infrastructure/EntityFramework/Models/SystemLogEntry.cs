using System.Text.Json;
using Interview4Create.Project.Infrastructure.EntityFramework.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Interview4Create.Project.Infrastructure.EntityFramework.Models;
public class SystemLogEntry
{
    public SystemLogEntry()
    {

    }

    public SystemLogEntry(EntityEntry entityEntry)
    {
        EntityEntry = entityEntry;
    }

    public int Id { get; set; }
    public EntityEntry EntityEntry { get; set; }
    public string TableName { get; set; }
    public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
    public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
    public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
    public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();
    public SystemLogType SystemLogType { get; set; }
    public List<string> ChangedColumns { get; } = new List<string>();

    public bool HasTemporaryProperties => TemporaryProperties.Any();

    public SystemLog ToSystemLog()
    {
        var log = new SystemLog()
        {
            Type = SystemLogType.ToString(),
            TableName = TableName,
            DateTime = DateTime.UtcNow,
            PrimaryKey = JsonSerializer.Serialize(KeyValues),
            OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues),
            NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues),
            AffectedColumns = ChangedColumns.Count == 0 ? null : JsonSerializer.Serialize(ChangedColumns)
        };

        return log;
    }

}
