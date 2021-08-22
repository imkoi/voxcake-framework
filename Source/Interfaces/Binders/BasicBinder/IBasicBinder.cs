using System;
using VoxCake.IoC;

namespace VoxCake.Framework
{
    public interface IBasicBinder
    {
        IBindingSequenceGeneric<TInstanceKey> Bind<TInstanceKey>() 
            where TInstanceKey : class;
        IBindingSequence Bind(Type bindingType);
        IBindingSequenceGeneric<TInstanceKey> BindInstance<TInstanceKey>(TInstanceKey instance) 
            where TInstanceKey : class;
        
        void Unbind<TBinding>();
        void Unbind(Type instanceType);
    }
}