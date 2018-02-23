using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Data.Dynamic;

namespace Forte.Migrations.EPiServer
{
    public class DynamicDataStoreMigrationLog : IMigrationLog
    {
        private const string MigrationsLogStoreName = "MigrationsLog";
        
        private readonly DynamicDataStore store = DynamicDataStoreFactory.Instance.GetStore(MigrationsLogStoreName) ?? DynamicDataStoreFactory.Instance.CreateStore(MigrationsLogStoreName, typeof(MigrationsLogEntry));

        public void Add(MigrationDescriptor migrationDescriptor)
        {
            this.store.Save(new MigrationsLogEntry
            {
                MigrationAppliedOn = DateTime.UtcNow,
                MigrationId = migrationDescriptor.Id,
                MigrationName = migrationDescriptor.Name
            });
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.store.Items<MigrationsLogEntry>().Select(e => e.MigrationId).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}