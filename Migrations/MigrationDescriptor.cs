using System;
using System.Collections.Generic;
using System.Linq;

namespace Forte.Migrations
{
    public abstract class MigrationDescriptor : IComparable<MigrationDescriptor>
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
        
        public int CompareTo(MigrationDescriptor other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            
            if (ReferenceEquals(this, other)) return 0;

            if (this.Dependencies.Contains(other.Id))
                return 1;

            if (other.Dependencies.Contains(this.Id))
                return -1;
            
            if (this.SequenceNo != null && other.SequenceNo != null)
            {
                var seqNoComparison = SequenceNo.Value.CompareTo(other.SequenceNo.Value);
                if (seqNoComparison != 0)
                    return seqNoComparison;
            }

            return string.Compare(Id, other.Id, StringComparison.OrdinalIgnoreCase);
        }
    }
}