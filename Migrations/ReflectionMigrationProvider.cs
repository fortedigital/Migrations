using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Forte.Migrations
{
    public class ReflectionMigrationProvider : IMigrationProvider
    {
        public static ReflectionMigrationProvider FromAssemblies(IEnumerable<Assembly> assemblies)
        {
            return new ReflectionMigrationProvider(assemblies); 
        }

        public static ReflectionMigrationProvider FromAssemblyOf<T>()
        {
            return FromAssemblyOf(typeof(T)); 
        }

        public static ReflectionMigrationProvider FromAssemblyOf(Type type)
        {
            return new ReflectionMigrationProvider(new [] { type.Assembly }); 
        }
        
        private readonly IEnumerable<Assembly> assemblies;

        private ReflectionMigrationProvider(IEnumerable<Assembly> assemblies)
        {
            this.assemblies = assemblies;
        }

        public IEnumerable<MigrationDescriptor> LoadMigrations()
        {
            return this.assemblies
                .SelectMany(a => a.GetExportedTypes())
                .Where(t => typeof(IMigration).IsAssignableFrom(t))
                .Select(t => new { Type = t, MigrationAttribute = t.GetCustomAttribute<MigrationAttribute>()})
                .Where(m => m.MigrationAttribute != null)
                .Select(m => new ReflectionMigrationDescriptor(m.Type, m.MigrationAttribute.MigrationId, m.Type.Name, m.MigrationAttribute.GetSequenceNoOrNull(), GetDependencies(m.Type)));
        }

        private static IEnumerable<string> GetDependencies(Type type)
        {
            return type
                .GetCustomAttributes<MigrationDependencyAttribute>()
                .Select(d => d.Dependency)
                .Select(t => new {Type = t, MigrationAttribute = t.GetCustomAttribute<MigrationAttribute>()})
                .Where(m => m.MigrationAttribute != null)
                .Select(m => m.MigrationAttribute.MigrationId);
        }

        private class ReflectionMigrationDescriptor : MigrationDescriptor
        {
            private readonly Type clrType;

            public ReflectionMigrationDescriptor(Type clrType, string id, string name, int? sequenceNo = null, IEnumerable<string> dependencies = null) : base(id, name, sequenceNo, dependencies)
            {
                this.clrType = clrType;
            }

            public override IMigration CreateMigration(IActivator<IMigration> activator)
            {
                return activator.CreateInstance(this.clrType);
            }
        }
    }
}