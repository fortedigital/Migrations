using System;

namespace Forte.Migrations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MigrationAttribute : Attribute
    {
        public string MigrationId { get; }
        public int? SequenceNo { get; set; }

        public MigrationAttribute(string migrationId)
        {
            this.MigrationId = migrationId;
        }
    }
}