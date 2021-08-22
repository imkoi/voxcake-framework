using VoxCake.IoC;

namespace VoxCake.Framework
{
    public interface ICommandPreFinalBindingSequence
    {
        IFinalBindingSequence AsSingle();
        void ToGlobal();
    }
}