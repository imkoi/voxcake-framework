namespace VoxCake.Framework
{
    public interface IInitializableQueue
    {
        void Add<TDependencyKey>();
    }
}