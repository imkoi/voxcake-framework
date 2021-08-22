using UnityEngine;
using VoxCake.IoC;

namespace VoxCake.Framework
{
    public interface IViewBindingSequence<TView>
    {
        IFinalBindingSequence To<TViewImplementation>() where TViewImplementation : MonoBehaviour, TView;
        IFinalBindingSequence ToInstance<TViewImplementation>(TViewImplementation value) 
            where TViewImplementation : MonoBehaviour, TView;
    }
}