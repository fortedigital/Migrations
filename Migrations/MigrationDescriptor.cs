using System.Collections.Generic;
using System.Linq;

namespace Forte.Migrations
{
    public abstract class MigrationDescriptor
    {
        public readonly string Id;
        public readonly string Name;
        public readonly int? SequenceNo;
        public readonly ISet<string> Dependencies;

        protected MigrationDescriptor(string id, string name, int? sequenceNo = null, IEnumerable<string> dependencies = null)
        {
            Id = id;
            this.Name = name;
            SequenceNo = sequenceNo;
            Dependencies = new HashSet<string>(dependencies ?? Enumerable.Empty<string>());
        }

        public abstract IMigration CreateMigration(IActivator<IMigration> activator);
    }
}