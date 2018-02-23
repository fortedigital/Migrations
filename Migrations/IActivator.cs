using System;

namespace Forte.Migrations
{
    public interface IActivator<T>
    {
        T CreateInstance(Type clrType);
    }
}