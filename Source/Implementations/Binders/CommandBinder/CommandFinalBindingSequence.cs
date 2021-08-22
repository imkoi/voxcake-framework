using VoxCake.Framework.Implementations.Binders;
using VoxCake.IoC;

namespace VoxCake.Framework.Implementations
{
    internal class CommandFinalBindingSequence<TObserver, TCommand> : CommandBindingBase, IFinalBindingSequence
    {
        private readonly IFinalBindingSequence _observerBindingSequence;

        public CommandFinalBindingSequence(
            CommandBuffer commandBuffer,
            CommandBinding commandBinding,
            IFinalBindingSequence observerBindingSequence)
            : base(commandBuffer, commandBinding)
        {
            _observerBindingSequence = observerBindingSequence;
        }
        
        public void AsGlobal()
        {
            base.AsGlobal<TObserver,TCommand>(_observerBindingSequence);
        }
    }
}