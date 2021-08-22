using System;
using VoxCake.Framework.Utilities;
using VoxCake.IoC;

namespace VoxCake.Framework.Implementations
{
    internal class ViewBinder : IViewBinder
    {
        private readonly IBindingsBuilder _binder;
        private readonly MonoBehaviourProvider _monoBehaviourProvider;
        
        public ViewBinder(Context context, IBindingsBuilder binder)
        {
            _binder = binder;
            _monoBehaviourProvider = new MonoBehaviourProvider(context);
        }
    
        public IViewBindingSequence<TView> Bind<TView>() where TView : class
        {
            var type = typeof(TView);
            if (!type.IsInterface)
            {
                throw new Exception("View should be binded to inteface!");
            }

            var bindingSequence = _binder.Bind<TView>();
            
            return new ViewBindingSequence<TView>(_monoBehaviourProvider, bindingSequence);
        }

        public void Unbind<TBinding>()
        {
            _binder.Unbind<TBinding>();
        }
    }
}