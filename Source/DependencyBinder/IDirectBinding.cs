using VoxCake.IoC;

namespace VoxCake.Framework
{
    public interface IDirectBinding
    {
        IDirectBinding Raw<T>();

        IDirectBinding Raw(object instance);

        IEndBinding To<T>();

        IEndBinding To(object instance);
    }
}