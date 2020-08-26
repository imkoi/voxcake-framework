using VoxCake.IoC;

namespace VoxCake.Framework
{
    public interface IDependencyBinding
    {
        IEndBinding As<T>();

        IEndBinding As(object instance);

        IDirectBinding Raw<T>();

        IDirectBinding Raw(object instance);

        IEndBinding To<T>();

        IEndBinding To(object instance);

        void ToGlobalContainer();
    }
}