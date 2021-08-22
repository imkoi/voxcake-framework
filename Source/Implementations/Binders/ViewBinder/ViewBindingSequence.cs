using System;
using UnityEngine;
using VoxCake.Framework.Utilities;
using VoxCake.IoC;

namespace VoxCake.Framework.Implementations
{
    internal class ViewBindingSequence<TView> : IViewBindingSequence<TView>
    {
        private const string TypeIsMonoBehaviourExceptionFormat = "{0} is not MonoBehaviour";
        
        private readonly MonoBehaviourProvider _monoBehaviourProvider;
        private readonly IBindingSequenceGeneric<TView> _bindingSequence;
        
        public ViewBindingSequence(MonoBehaviourProvider monoBehaviourProvider,
            IBindingSequenceGeneric<TView> bindingSequence)
        {
            _monoBehaviourProvider = monoBehaviourProvider;
            _bindingSequence = bindingSequence;
        }

        public IFinalBindingSequence To<TViewImplementation>() where TViewImplementation : MonoBehaviour, TView
        {
            var type = typeof(TViewImplementation);

            if (!_monoBehaviourProvider.IsMonoBehaviour(type))
            {
                throw new Exception(string.Format(TypeIsMonoBehaviourExceptionFormat,type.Name));
            }
            
            var view = (TViewImplementation)_monoBehaviourProvider.GetMonoBehaviour(type);
            
            return ToInstance(view);
        }

        public IFinalBindingSequence ToInstance<TViewImplementation>(TViewImplementation instance) 
            where TViewImplementation : MonoBehaviour, TView
        {
            var endlessBindingSequence = _bindingSequence.ToInstance(instance);

            return endlessBindingSequence.AsSingle();
        }
    }
}