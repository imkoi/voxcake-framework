using VoxCake.Framework.Utilities;
using VoxCake.IoC;

namespace VoxCake.Framework
{
    internal class DependencyBinding : MonoBehaviourCache, IDependencyBinding
    {
        private readonly MonoBehaviourStateData _state;
        private readonly IBinding _binding;

        public DependencyBinding(MonoBehaviourStateData state, IBinding binding) 
            : base(state)
        {
            _state = state;
            _binding = binding;
        }

        public IEndBinding As<T>()
        {
            var type = typeof(T);
            
            if (IsMonoBehaviour(type))
            {
                _binding.As(GetMonoBehaviour(type));
            }

            return _binding.As<T>();
        }

        public IEndBinding As(object instance)
        {
            TryCacheIfMonoBehaviour(instance);
            return _binding.As(instance);
        }

        IDirectBinding IDependencyBinding.Raw<T>()
        {
            var type = typeof(T);
            
            if (IsMonoBehaviour(type))
            {
                return new DirectBinding(_state, _binding.Raw(GetMonoBehaviour(type)));
            }

            return new DirectBinding(_state, _binding.Raw<T>());
        }

        public IDirectBinding Raw(object instance)
        {
            TryCacheIfMonoBehaviour(instance);
            return new DirectBinding(_state, _binding.Raw(instance));
        }

        IEndBinding IDependencyBinding.To<T>()
        {
            var type = typeof(T);
            
            if (IsMonoBehaviour(type))
            {
                return _binding.To(GetMonoBehaviour(type));
            }

            return _binding.To<T>();
        }

        public IEndBinding To(object instance)
        {
            TryCacheIfMonoBehaviour(instance);
            return _binding.To(instance);
        }

        public void ToGlobalContainer()
        {
            _binding.ToGlobalContainer();
        }
    }
}