namespace DotCommon.DependencyInjection
{
    public interface IObjectAccessor<out T>
    {
        T? Value { get; }
    }
}
