﻿namespace Sqlbi.Bravo.Models
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public abstract class ExportDataEntity
    {
        [JsonPropertyName("status")]
        public ExportDataStatus Status { get; set; }

        public void SetRunning() => Status = ExportDataStatus.Running;

        public void SetCompleted() => Status = ExportDataStatus.Completed;

    }

    public class ExportDataJob : ExportDataEntity
    {
        [JsonPropertyName("tables")]
        public HashSet<ExportDataTable> Tables { get; set; } = new();

        public void SetCanceled() => Status = ExportDataStatus.Canceled;

        public void SetFailed() => Status = ExportDataStatus.Failed;
    }

    public class ExportDataTable : ExportDataEntity
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("rows")]
        public int Rows { get; set; } = 0;

        public void SetTruncated() => Status = ExportDataStatus.Truncated;
    }

    internal static class ExportDataJobExtensions
    {
        public static ExportDataTable AddNew(this ExportDataJob job, string name)
        {
            var table = new ExportDataTable
            {
                Name = name,
            };

            job.Tables.Add(table);
            table.SetRunning();

            return table;
        }
    }
}
