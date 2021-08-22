using System;
using VoxCake.IoC;

namespace VoxCake.Framework.Implementations
{
    internal class BasicBinder : IBasicBinder
    {
        private readonly IBindingsBuilder _bindingsBuilder;
        
        public BasicBinder(IBindingsBuilder bindingsBuilder)
        {
            _bindingsBuilder = bindingsBuilder;
        }

        public IBindingSequenceGeneric<TInstanceKey> Bind<TInstanceKey>() 
            where TInstanceKey : class
        {
            return _bindingsBuilder.Bind<TInstanceKey>();
        }
        
        public IBindingSequence Bind(Type bindingType)
        {
            return _bindingsBuilder.Bind(bindingType);
        }

        public IBindingSequenceGeneric<TInstanceKey> BindInstance<TInstanceKey>(TInstanceKey instance) 
            where TInstanceKey : class
        {
            return _bindingsBuilder.BindInstance(instance);
        }

        public void Unbind<TBinding>()
        {
            _bindingsBuilder.Unbind<TBinding>();
        }

        public void Unbind(Type instanceType)
        {
            _bindingsBuilder.Unbind(instanceType);
        }
    }
}