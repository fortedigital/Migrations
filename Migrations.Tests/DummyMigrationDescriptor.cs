using System.Collections.Generic;

namespace Forte.Migrations.Tests
{
    class DummyMigrationDescriptor : MigrationDescriptor
    {
        public DummyMigrationDescriptor(string id, string name, int? sequenceNo = null, IEnumerable<string> dependencies = null) : base(id, name, sequenceNo, dependencies)
        {
        }

        public override IMigration CreateMigration(IActivator<IMigration> activator)
        {
            throw new System.NotImplementedException();
        }
    }
}