using System;

namespace Forte.Migrations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MigrationDependencyAttibute : Attribute
    {
        public readonly Type Dependency;

        public MigrationDependencyAttibute(Type dependency)
        {
            this.Dependency = dependency;
        }
    }
}