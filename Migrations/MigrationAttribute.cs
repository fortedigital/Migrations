using System;

namespace Forte.Migrations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MigrationAttribute : Attribute
    {
        public string MigrationId { get; }
        public int SequenceNo { get; set; } = -1;

        public MigrationAttribute(string migrationId)
        {
            this.MigrationId = migrationId;
        }

        public int? GetSequenceNoOrNull()
        {
            return this.SequenceNo >= 0 ? (int?)this.SequenceNo : null;
        }
    }
}