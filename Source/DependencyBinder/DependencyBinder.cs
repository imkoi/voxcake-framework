using VoxCake.Framework.Utilities;
using VoxCake.IoC;

namespace VoxCake.Framework
{
    internal class DependencyBinder : MonoBehaviourCache, IDependencyBinder
    {
        private readonly MonoBehaviourStateData _state;
        private readonly IBinder _binder;

        public DependencyBinder(MonoBehaviourStateData state, IBinder binder) 
            : base(state)
        {
            _state = state;
            _binder = binder;
        }
    
        public IDependencyBinding Bind<T>()
        {
            var type = typeof(T);
            IBinding binding;
            if (IsMonoBehaviour(type))
            {
                binding = _binder.Bind(GetMonoBehaviour(type));
            }
            else
            {
                binding = _binder.Bind<T>();
            }
            
            return new DependencyBinding(_state, binding);
        }

        public IDependencyBinding Bind(object instance)
        {
            TryCacheIfMonoBehaviour(instance);
            var binding = _binder.Bind(instance);

            return new DependencyBinding(_state, binding);
        }
    }
}