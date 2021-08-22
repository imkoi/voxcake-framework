using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoxCake.Framework.Utilities
{
    internal class MonoBehaviourProvider
    {
        private const string NoMonoExceptionFormat = "{0} was not found inside {1}!" +
                                                     "Check that {2} is a child of {3}";
        
        private readonly Context _context;
        private readonly string _contextName;
        private readonly Dictionary<Type, object> _cache;

        internal MonoBehaviourProvider(Context context)
        {
            _context = context;
            _contextName = context.GetType().Name;
            _cache = new Dictionary<Type, object>();
        }

        public object GetMonoBehaviour(Type type)
        {
            object mono;
            if (_cache.ContainsKey(type))
            {
                mono = _cache[type];
            }
            else
            {
                mono = _context.GetComponentInChildren(type);
                
                if(mono == null)
                {
                    var monoName = type.Name;
                    throw new Exception(string.Format(NoMonoExceptionFormat,
                        monoName, _contextName, monoName, _contextName));
                }
                
                CacheMonoBehaviour(type, mono);
            }

            return mono;
        }

        public bool TryCacheIfMonoBehaviour(object instance)
        {
            var isCached = false;

            var type = instance.GetType();
            if (instance is MonoBehaviour)
            {
                if (!_cache.ContainsKey(type))
                {
                    CacheMonoBehaviour(type, instance);
                    isCached = true;
                }
            }

            return isCached;
        }

        public bool IsMonoBehaviour(Type type)
        {
            if (type.IsSubclassOf(typeof(MonoBehaviour)))
            {
                return true;
            }

            return false;
        }

        private void CacheMonoBehaviour(Type type, object mono)
        {
            _cache.Add(type, mono);
        }
    }
}