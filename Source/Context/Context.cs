using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using VoxCake.Common.Utilities;
using VoxCake.Framework.Utilities;
using VoxCake.IoC;

namespace VoxCake.Framework
{
    public abstract class Context : MonoBehaviour
    {
        protected event Action<IDependencyBinder> BindDependencies;
        protected event Action<ICommandBinder> BindCommands;
        protected event Action<IInitializableQueue> AddSequenceInitializables;
        protected event Action<IInitializableQueue> AddParallelInitializables;
        
        protected float Progress => _container.ResolveProgress;
        protected abstract void OnInitialize();
        
        private IContainer _container;
        private TickableObject _tickableObject;
        private InitializableQueue _sequenceInitializableQueue;
        private InitializableQueue _parallelInitializableQueue;
        private CancellationTokenSource _tokenSource;

        private void Awake()
        {
            _tokenSource = new CancellationTokenSource();
            
            _container = new Container(this);
            _tickableObject = new TickableObject();

            _sequenceInitializableQueue = new InitializableQueue(_container.Dependencies);
            _parallelInitializableQueue = new InitializableQueue(_container.Dependencies);
            
            _container.BindDependencies += OnBindDependencies;

            OnInitialize();
        }

        private void Update()
        {
            _tickableObject.InvokeTick(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _tokenSource.Cancel();
            _container.Dispose();
        }
        
        protected void SetToken<TContextToken>()
        {
            _container.SetToken<TContextToken>();
        }
        
        protected TDependency GetDependency<TDependency>()
        {
            return _container.GetDependency<TDependency>();
        }

        protected async Task WaitForContextResolveAsync<TContextToken>()
        {
            await _container.WaitForContainerResolveAsync<TContextToken>();
        }

        protected async Task ResolveAsync(int maxTaskFreezeMs = 16)
        {
            await _container.ResolveDependenciesAsync(maxTaskFreezeMs);
            await InitializeDependenciesAsync(maxTaskFreezeMs);
        }

        protected async Task LoadContextAsync(ContextLoadingData contextLoadingData, int maxTaskFreezeMs = 16)
        {
            var sw = Stopwatch.StartNew();
            
            var unloadScenes = contextLoadingData.unloadScenes;
            if (unloadScenes != null)
            {
                await SceneLoadingUtility.UnloadScenes(unloadScenes, sw, maxTaskFreezeMs, _tokenSource.Token);
            }

            await SceneLoadingUtility.LoadScene(contextLoadingData.sceneName, sw, maxTaskFreezeMs, _tokenSource.Token);

            await Awaiter.ReduceTaskFreezeAsync(sw, maxTaskFreezeMs, CancellationToken.None);
        }
        
        private async Task InitializeDependenciesAsync(int maxTaskFreezeMs)
        {
            var sw = Stopwatch.StartNew();
            
            AddParallelInitializables?.Invoke(_parallelInitializableQueue);
            AddSequenceInitializables?.Invoke(_sequenceInitializableQueue);
            var parallelInitializableTask = _parallelInitializableQueue.InitializeDependenciesInParallelAsync(sw,
                maxTaskFreezeMs, _tokenSource.Token);
            var sequenceInitializableTask = _sequenceInitializableQueue.InitializeDependenciesInSequenceAsync(sw,
                maxTaskFreezeMs, _tokenSource.Token);

            await Task.WhenAll(parallelInitializableTask, sequenceInitializableTask);

            _parallelInitializableQueue = null;
            _sequenceInitializableQueue = null;
        }

        private void OnBindDependencies(IBinder binder)
        {
            binder.Bind<ITickable>().As(_tickableObject);

            var state = new MonoBehaviourStateData(this, new Dictionary<Type, object>());
            var dependencyBinder = new DependencyBinder(state, binder);
            BindDependencies?.Invoke(dependencyBinder);
        }
    }
}