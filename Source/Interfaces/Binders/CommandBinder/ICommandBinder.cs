namespace VoxCake.Framework
{
    public interface ICommandBinder
    {
        ICommandBindingSequence<TObserver> Bind<TObserver>() where TObserver : ObserverBase;
        
        void Unbind<TBinding>() where TBinding : ObserverBase;
    }
}