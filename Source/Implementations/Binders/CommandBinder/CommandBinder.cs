using VoxCake.Framework.Implementations.Binders;
using VoxCake.IoC;

namespace VoxCake.Framework.Implementations
{
    internal class CommandBinder : ICommandBinder
    {
        private readonly IBindingsBuilder _bindingsBuilder;
        private readonly CommandBuffer _commandBuffer;
        
        public CommandBinder(IBindingsBuilder bindingsBuilder, CommandBuffer commandBuffer)
        {
            _bindingsBuilder = bindingsBuilder;
            _commandBuffer = commandBuffer;
        }
        
        public ICommandBindingSequence<TObserver> Bind<TObserver>() where TObserver : ObserverBase
        {
            var observerBindingSequence = _bindingsBuilder.Bind<TObserver>();
            
            return new CommandBindingSequence<TObserver>(
                _commandBuffer,
                new CommandBinding(),
                observerBindingSequence);
        }

        public void Unbind<TBinding>() where TBinding : ObserverBase
        {
            _commandBuffer.Unbind(typeof(TBinding));
            _bindingsBuilder.Unbind<TBinding>();
        }
    }
}