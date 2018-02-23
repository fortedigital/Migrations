using System;
using System.Collections.Generic;

namespace Forte.Migrations
{
    public class DecoratingActivator<T> : IActivator<T>
    {
        private readonly IActivator<T> activator;
        private readonly IEnumerable<IDecoratorProvider<T>> decoratorProviders;

        public DecoratingActivator(params IDecoratorProvider<T>[] decoratorProviders) : this(new ReflectionActivator<T>(), decoratorProviders)
        {
        }

        public DecoratingActivator(IActivator<T> activator, params IDecoratorProvider<T>[] decoratorProviders)
        {
            this.activator = activator;
            this.decoratorProviders = decoratorProviders;
        }

        public T CreateInstance(Type type)
        {
            var instance = this.activator.CreateInstance(type);
            foreach (var decorator in this.decoratorProviders)
            {
                instance = decorator.Decorate(instance);
            }

            return instance;
        }
    }
}