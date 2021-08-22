using VoxCake.Framework.Implementations.Binders;
using VoxCake.IoC;

namespace VoxCake.Framework.Implementations
{
    internal class CommandBindingSequence<TObserver> : CommandBindingBase, ICommandBindingSequence<TObserver>
        where TObserver : ObserverBase
    {
        private readonly IBindingSequenceGeneric<TObserver> _observerBindingSequence;

        public CommandBindingSequence(
            CommandBuffer commandBuffer,
            CommandBinding commandBinding,
            IBindingSequenceGeneric<TObserver> observerBindingSequence) 
            : base(commandBuffer, commandBinding)
        {
            _observerBindingSequence = observerBindingSequence;
        }
        
        public ICommandPreFinalBindingSequence To<TCommand>() where TCommand : ICommand
        {
            return base.To<TObserver, TCommand>(_observerBindingSequence);
        }
    }
}