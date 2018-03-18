using System;
using EPiServer.ServiceLocation;
using StructureMap;

namespace Forte.Migrations.EPiServer
{
    public class ServiceLocatorActivator : IActivator<IMigration>
    {
        private readonly IServiceLocator serviceLocator;

        public ServiceLocatorActivator(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public IMigration CreateInstance(Type clrType)
        {
            return (IMigration) this.serviceLocator.GetInstance(clrType);
        }
    }
}