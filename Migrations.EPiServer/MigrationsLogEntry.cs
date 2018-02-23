using System;
using EPiServer.Data;

namespace Forte.Migrations.EPiServer
{
    public class MigrationsLogEntry
    {
        public Identity Id { get; set; }

        public string MigrationName { get; set; }
        public string MigrationId { get; set; }
        public DateTime MigrationAppliedOn { get; set; }

        public MigrationsLogEntry()
        {
            this.Id = Identity.NewIdentity();
        }
    }
}