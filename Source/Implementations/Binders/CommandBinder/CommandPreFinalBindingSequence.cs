using VoxCake.Framework.Implementations.Binders;
using VoxCake.IoC;

namespace VoxCake.Framework.Implementations
{
    internal class CommandPreFinalBindingSequence<TObserver, TCommand> 
        : CommandBindingBase, ICommandPreFinalBindingSequence
    {
        private readonly IBindingSequenceGeneric<TObserver> _observerBindingSequence;

        internal CommandPreFinalBindingSequence(
            CommandBuffer commandBuffer,
            CommandBinding commandBinding,
            IBindingSequenceGeneric<TObserver> observerBindingSequence)
            : base(commandBuffer, commandBinding)
        {
            _observerBindingSequence = observerBindingSequence;
        }
        
        public IFinalBindingSequence AsSingle()
        {
            return base.AsSingle<TObserver, TCommand>(_observerBindingSequence);
        }

        public void ToGlobal()
        {
            base.AsGlobal<TObserver, TCommand>(_observerBindingSequence);
        }
    }
}