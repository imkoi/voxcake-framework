using System;
using System.Collections.Generic;

namespace VoxCake.Framework.Utilities
{
    internal struct MonoBehaviourStateData
    {
        internal readonly Context context;
        internal readonly Dictionary<Type, object> cache;

        internal MonoBehaviourStateData(Context context, Dictionary<Type, object> cache)
        {
            this.context = context;
            this.cache = cache;
        }
    }
}