using System;
using System.Collections.Generic;
using System.Linq;

namespace Forte.Migrations.Tests
{
    public class MigrationSorter
    {
        private readonly IReadOnlyCollection<MigrationDescriptor> input;

        public MigrationSorter(IEnumerable<MigrationDescriptor> input)
        {
            this.input = new List<MigrationDescriptor>(input);
        }

        public IEnumerable<MigrationDescriptor> Sort()
        {
            var remaining = new List<MigrationDescriptor>(input);
            var result = new List<MigrationDescriptor>();

            while (remaining.Count > 0)
            {
                var minimum = remaining.FirstOrDefault();
                foreach (var s in remaining)
                {
                    if (IsBefore(s, minimum))
                    {
                        minimum = s;
                    }
                }
                result.Add(minimum);
                remaining.Remove(minimum);
            }

            return result;
        }

        private bool IsBefore(MigrationDescriptor x, MigrationDescriptor y)
        {
            if (x == y)
            {
                return false;
            }

            if (DependsOn(x, y))
            {
                return false;
            }

            if (DependsOn(y, x))
            {
                return true;
            }

            if (x.SequenceNo != null && y.SequenceNo != null && x.SequenceNo < y.SequenceNo)
            {
                return x.SequenceNo < y.SequenceNo;
            }

            return string.Compare(x.Id, y.Id, StringComparison.Ordinal) < 0;
        }

        private bool DependsOn(MigrationDescriptor x, MigrationDescriptor y)
        {
            return DependsOn(x.Id, y.Id);
        }

        private bool DependsOn(string x, string y)
        {
            if (x == y)
            {
                throw new ArgumentException($"Cyclic dependency for {x}");
            }

            if (GetMigration(x).Dependencies.Contains(y))
            {
                return true;
            }

            return GetMigration(x).Dependencies.Any(dependency => DependsOn(dependency,y));
        }

        private MigrationDescriptor GetMigration(string migrationId)
        {
            var migration= this.input.SingleOrDefault(x => x.Id == migrationId);
            if (migration == null)
            {
                throw new ArgumentException($"Migration {migrationId} not found.",nameof(migrationId));
            }

            return migration;
        }
    }
}