using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using VoxCake.Common.Utilities;

namespace VoxCake.Framework
{
    internal class InitializableQueue : IInitializableQueue
    {
        private readonly Queue<Type> _initializables;
        private readonly Dictionary<Type, object> _dependencies;

        public InitializableQueue(Dictionary<Type, object> dependencies)
        {
            _initializables = new Queue<Type>();
            _dependencies = dependencies;
        }
        
        void IInitializableQueue.Add<TDependencyKey>()
        {
            _initializables.Enqueue(typeof(TDependencyKey));
        }

        internal async Task InitializeDependenciesInSequenceAsync(Stopwatch sw, int maxTaskFreezeMs,
            CancellationToken cancellationToken)
        {
            var dependencies = await GetDependenciesAsync(sw, maxTaskFreezeMs, cancellationToken);
            
            foreach (var dependency in dependencies)
            {
                if (dependency is IInitializable initializable)
                {
                    await initializable.Initialize();
                }
            }
        }
        
        internal async Task InitializeDependenciesInParallelAsync(Stopwatch sw, int maxTaskFreezeMs,
            CancellationToken cancellationToken)
        {
            var dependencies = await GetDependenciesAsync(sw, maxTaskFreezeMs, cancellationToken);
            var initializables = new List<Task>();
            
            foreach (var dependency in dependencies)
            {
                if (dependency is IInitializable initializable)
                {
                    initializables.Add(initializable.Initialize());
                    await Awaiter.ReduceTaskFreezeAsync(sw, maxTaskFreezeMs, cancellationToken);
                }
            }

            await Task.WhenAll(initializables.ToArray());
        }

        private async Task<object[]> GetDependenciesAsync(Stopwatch sw, int maxTaskFreezeMs,
            CancellationToken cancellationToken)
        {
            var initalizablesCount = _initializables.Count;
            var dependencies = new object[initalizablesCount];
            
            for (int i = 0; i < initalizablesCount; i++)
            {
                var initializableKey = _initializables.Dequeue();
                if (_dependencies.ContainsKey(initializableKey))
                {
                    dependencies[i] = _dependencies[initializableKey];
                    await Awaiter.ReduceTaskFreezeAsync(sw, maxTaskFreezeMs, cancellationToken);
                }
            }

            return dependencies;
        }
    }
}