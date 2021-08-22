using System;
using UnityEngine;
using VoxCake.Framework.Implementations;
using VoxCake.IoC;

namespace VoxCake.Framework
{
    public abstract class Context : MonoBehaviour, IContext
    {
        private const string CONTEXT_INITIALIZE_FAIL_MESSAGE = "Failed to initialize context \"{0}\" ";

        public IBasicBinder BasicBinder
        {
            get
            {
                return GetBasicBinder();
            }
        }
        
        public IViewBinder ViewBinder
        {
            get
            {
                return GetViewBinder();
            }
        }
        
        public ICommandBinder CommandBinder 
        {
            get
            {
                return GetCommandBinder();
            }
        }

        public IDomainManager DomainManager
        {
            get
            {
                return GetDomainManager();
            }
        }

        protected abstract void Install();
        
        private IContainer _container;
        private IBasicBinder _basicBinder;
        private IViewBinder _viewBinder;
        private ICommandBinder _commandBinder;
        
        private TickableObject _tickableObject;
        private IDomainManager _domainManager;
        private CommandBuffer _commandBuffer;

        private void Awake()
        {
            var contextType = GetType();
            
            try
            {
                _container = new Container(contextType);
                _container.InstanceConfigured += OnInstanceConfigured;
                
                _tickableObject = new TickableObject();
                
                BindDefault(_container.BindingsBuilder);
                
                Install();
            }
            catch (Exception exception)
            {
                throw new FrameworkException(
                    string.Format(CONTEXT_INITIALIZE_FAIL_MESSAGE, contextType.Name), exception);
            }
        }

        private void Update()
        {
            _tickableObject.InvokeTick(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            _tickableObject.InvokeFixedTick(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            _tickableObject.InvokeLateTick(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _container.InstanceConfigured -= OnInstanceConfigured;
            _container.Dispose();
        }
        
        public TInstance GetInstance<TInstance>()
        {
            var instanceType = typeof(TInstance);
            var instance = GetInstance(instanceType);

            return (TInstance)instance;
        }

        public object GetInstance(Type instanceType)
        {
            var instance = _container.GetInstance(instanceType);
            
            return instance;
        }

        private void BindDefault(IBindingsBuilder bindingsBuilder)
        {
            var coroutineController = new CoroutineController(this);
            
            bindingsBuilder.Bind<ITickable>().ToInstance(_tickableObject).AsSingle();
            bindingsBuilder.Bind<ICoroutineController>().ToInstance(coroutineController).AsSingle();
        }

        private void OnInstanceConfigured(object instance)
        {
            if (_commandBuffer != null)
            {
                var observer = instance as ObserverBase;
                if (observer != null && _commandBuffer.HasCommandForObserver(observer))
                {
                    _commandBuffer.SubscribeCommandOnObserver(observer);
                }
            }
        }

        private IBasicBinder GetBasicBinder()
        {
            if (_basicBinder == null)
            {
                _basicBinder = new BasicBinder(_container.BindingsBuilder);
            }

            return _basicBinder;
        }

        private IViewBinder GetViewBinder()
        {
            if (_viewBinder == null)
            {
                _viewBinder = new ViewBinder(this, _container.BindingsBuilder);
            }

            return _viewBinder;
        }

        private ICommandBinder GetCommandBinder()
        {
            if (_commandBinder == null)
            {
                _commandBuffer = new CommandBuffer(this);
                _commandBinder = new CommandBinder(_container.BindingsBuilder, _commandBuffer);
            }

            return _commandBinder;
        }

        private IDomainManager GetDomainManager()
        {
            if (_domainManager == null)
            {
                _domainManager = new DomainManager(this);
            }

            return _domainManager;
        }

        internal void AddView(View view)
        {
            var viewType = view.GetType();
            var viewInterfaces = viewType.GetInterfaces();

            if (viewInterfaces.Length > 0)
            {
                BasicBinder.Bind(viewInterfaces[0]).ToInstance(view);
            }
            else
            {
                BasicBinder.BindInstance(view);
            }
        }
        
        internal void RemoveView(View view)
        {
            var viewType = view.GetType();
            var viewInterfaces = viewType.GetInterfaces();

            if (viewInterfaces.Length > 0)
            {
                BasicBinder.Unbind(viewInterfaces[0]);
            }
            else
            {
                BasicBinder.Unbind(viewType);
            }
        }
    }
}