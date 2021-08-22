using VoxCake.Framework.Implementations.Binders;
using VoxCake.IoC;

namespace VoxCake.Framework.Implementations
{
    internal class CommandBindingBase
    {
        private readonly CommandBuffer _commandBuffer;
        private readonly CommandBinding _commandBinding;

        internal CommandBindingBase(CommandBuffer commandBuffer, CommandBinding commandBinding)
        {
            _commandBuffer = commandBuffer;
            _commandBinding = commandBinding;
        }

        protected ICommandPreFinalBindingSequence To<TObserver, TCommand>(
            IBindingSequenceGeneric<TObserver> observerBindingSequence)
        {
            _commandBinding.commandType = typeof(TCommand);
            
            return new CommandPreFinalBindingSequence<TObserver, TCommand>(
                _commandBuffer,
                _commandBinding,
                observerBindingSequence);
        }
        
        protected IFinalBindingSequence AsSingle<TObserver, TCommand>(
            IBindingSequenceGeneric<TObserver> observerBindingSequence)
        {
            var preFinalBindingSequence = observerBindingSequence.AsSingle();
            _commandBinding.isSingle = true;
            
            return new CommandFinalBindingSequence<TObserver, TCommand>(
                _commandBuffer,
                _commandBinding,
                preFinalBindingSequence);
        }

        protected void AsGlobal<TObserver, TCommand>(IBindingSequenceGeneric<TObserver> observerBindingSequence)
        {
            observerBindingSequence.AsGlobal();
            _commandBinding.isGlobal = true;

            _commandBuffer.Bind(typeof(TObserver), _commandBinding);
        }
        
        protected void AsGlobal<TObserver, TCommand>(IFinalBindingSequence observerBindingSequence)
        {
            observerBindingSequence.AsGlobal();
            _commandBinding.isGlobal = true;

            _commandBuffer.Bind(typeof(TObserver), _commandBinding);
        }
    }
}