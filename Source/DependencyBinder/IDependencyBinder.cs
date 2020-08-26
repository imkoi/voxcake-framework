namespace VoxCake.Framework
{
    public interface IDependencyBinder
    {
        IDependencyBinding Bind<T>();
        IDependencyBinding Bind(object instance);
    }
}