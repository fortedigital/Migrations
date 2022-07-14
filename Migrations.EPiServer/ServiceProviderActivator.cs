using System;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Asn1.X509;

namespace Forte.Migrations.EPiServer
{
    public class ServiceProviderActivator : IActivator<IMigration>
    {
        private readonly IServiceProvider serviceProvider;

        public ServiceProviderActivator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IMigration CreateInstance(Type clrType)
        {
            return (IMigration) ActivatorUtilities.CreateInstance(serviceProvider, clrType);
        }
    }
}