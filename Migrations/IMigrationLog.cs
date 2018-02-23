using System;
using System.Collections.Generic;

namespace Forte.Migrations
{
    public interface IMigrationLog : IEnumerable<string>
    {
        void Add(MigrationDescriptor migrationDescriptor);
    }
}