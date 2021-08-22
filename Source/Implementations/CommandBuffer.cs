using System;
using System.Collections.Generic;
using VoxCake.Framework.Implementations.Binders;

namespace VoxCake.Framework.Implementations
{
    public class CommandBuffer
    {
        private readonly IContext _context;
        private Dictionary<Type, CommandBinding> _bindings;

        public CommandBuffer(IContext context)
        {
            _context = context;
            _bindings = new Dictionary<Type, CommandBinding>();
        }

        public void SubscribeCommandOnObserver(ObserverBase observerBase)
        {
            observerBase.BeforeDispatch = null;
            observerBase.BeforeDispatch += value =>
            {
                var commandBinding = _bindings[observerBase.GetType()];
                var commandType = commandBinding.commandType;
                var isSingle = commandBinding.isSingle;
                var isGlobal = commandBinding.isGlobal;
                    
                var commandBind = _context.BasicBinder.Bind(commandBinding.commandType);
                    
                if (value != null)
                {
                    commandBind.WithInstance(value);
                }

                if (isSingle)
                {
                    commandBind.AsSingle();
                }

                if (isGlobal)
                {
                    commandBind.AsGlobal();
                }

                var commandInstance = _context.GetInstance(commandType);

                var command = commandInstance as ICommand;
                if (command != null)
                {
                    command.Execute();

                    var disposableCommand = command as IDisposable;
                    if (!commandBinding.isSingle && disposableCommand != null)
                    {
                        disposableCommand.Dispose();
                    }
                }

                _context.BasicBinder.Unbind(commandType);
            };
        }

        public bool HasCommandForObserver(ObserverBase observer)
        {
            var hasCommand = _bindings.ContainsKey(observer.GetType());
            
            return hasCommand;
        }

        public void Bind(Type observerType, CommandBinding commandBinding)
        {
            _bindings.Add(observerType, commandBinding);
        }

        public void Unbind(Type observerType)
        {
            _bindings.Remove(observerType);
        }
    }
}