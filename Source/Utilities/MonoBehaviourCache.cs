using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoxCake.Framework.Utilities
{
    internal class MonoBehaviourCache
    {
        private readonly Context _context;
        private readonly string _contextName;
        private readonly Dictionary<Type, object> _cache;

        internal MonoBehaviourCache(MonoBehaviourStateData stateData)
        {
            _context = stateData.context;
            _contextName = stateData.context.GetType().Name;
            _cache = stateData.cache;
        }
        
        protected object GetMonoBehaviour(Type type)
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
                    throw new Exception($"{monoName} was not found inside {_contextName}!" +
                                        $"Check that {monoName} is a child of {_contextName}");
                }
                
                CacheMonoBehaviour(type, mono);
            }

            return mono;
        }

        protected bool TryCacheIfMonoBehaviour(object instance)
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

        protected bool IsMonoBehaviour(Type type)
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