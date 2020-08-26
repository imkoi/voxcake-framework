using System;

namespace VoxCake.Framework
{
    public abstract class Observer
    {
        private Action _listener;

        public void Dispatch()
        {
            _listener?.Invoke();
        }

        public void AddListener(Action listener)
        {
            _listener += listener;
        }

        public void RemoveListener(Action listener)
        {
            _listener -= listener;
        }
        
        protected void RemoveAllListeners()
        {
            _listener = null;
        }
    }

    public abstract class Observer<TParameter>
    {
        private Action<TParameter> _listener;

        public void Dispatch(TParameter value)
        {
            _listener?.Invoke(value);
        }
        
        public void AddListener(Action<TParameter> listener)
        {
            _listener += listener;
        }

        public void RemoveListener(Action<TParameter> listener)
        {
            _listener -= listener;
        }

        protected void RemoveAllListeners()
        {
            _listener = null;
        }
    }
}