using System;
using System.Threading.Tasks;

namespace Forte.Migrations
{
    public interface IMigrationSynchronizationContext
    {
        Task<TResult> RunSynchronizedAsync<TResult>(Func<Task<TResult>> action);
    }

    public static class MigrationSynchronizationContextExtensions
    {
        public static Task RunSynchronizedAsync(this IMigrationSynchronizationContext context, Func<Task> action)
        {
            return context.RunSynchronizedAsync<object>(async () =>
            {
                await action();
                return null;
            });
        }
    }
}