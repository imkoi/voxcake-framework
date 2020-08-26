using VoxCake.Framework.Utilities;
using VoxCake.IoC;

namespace VoxCake.Framework
{
    internal class DirectBinding : MonoBehaviourCache, IDirectBinding
    {
        private readonly MonoBehaviourStateData _state;
        private readonly IRawBinding _binding;
        
        internal DirectBinding(MonoBehaviourStateData state, IRawBinding binding) : base(state)
        {
            _state = state;
            _binding = binding;
        }

        public IDirectBinding Raw<T>()
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

        public IEndBinding To<T>()
        {
            var type = typeof(T);
            
            if (IsMonoBehaviour(type))
            {
                return _binding.To<T>();
            }

            return _binding.To<T>();
        }

        public IEndBinding To(object instance)
        {
            TryCacheIfMonoBehaviour(instance);
            return _binding.To(instance);
        }
    }
}