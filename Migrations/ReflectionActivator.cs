using System;

namespace Forte.Migrations
{
    public class ReflectionActivator<T> : IActivator<T>
    {
        public T CreateInstance(Type clrType)
        {
            if (typeof(T).IsAssignableFrom(clrType) == false)
                throw new InvalidOperationException($"Type '{typeof(T)}' is not assignable from provided clrType '{clrType}'");
            
            return (T)Activator.CreateInstance(clrType);
        }
    }
}