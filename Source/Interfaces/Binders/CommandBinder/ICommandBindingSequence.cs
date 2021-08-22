namespace VoxCake.Framework
{
    public interface ICommandBindingSequence<TObserver> where TObserver : ObserverBase
    {
        ICommandPreFinalBindingSequence To<TCommand>() where TCommand : ICommand;
    }
}