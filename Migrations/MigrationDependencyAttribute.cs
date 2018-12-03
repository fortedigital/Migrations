using System;

namespace Forte.Migrations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MigrationDependencyAttribute : Attribute
    {
        public readonly Type Dependency;

        public MigrationDependencyAttribute(Type dependency)
        {
            this.Dependency = dependency;
        }
    }
}