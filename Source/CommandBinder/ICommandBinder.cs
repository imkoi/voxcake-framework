namespace VoxCake.Framework
{
    public interface ICommandBinder
    {
        ICommandBinding Bind<TObserver>() where TObserver : Observer;
    }
}