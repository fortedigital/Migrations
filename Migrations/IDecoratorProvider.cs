namespace Forte.Migrations
{
    public interface IDecoratorProvider<T>
    {
        T Decorate(T instance);
    }
}