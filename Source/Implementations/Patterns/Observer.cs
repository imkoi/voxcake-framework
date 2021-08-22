using System;

namespace VoxCake.Framework
{
    public abstract class Observer : ObserverBase
    {
        private event Action Listener;

        public void Dispatch()
        {
            if (BeforeDispatch != null)
            {
                BeforeDispatch.Invoke(null);
            }

            if (Listener != null)
            {
                Listener.Invoke();
            }
        }

        public void AddListener(Action listener)
        {
            Listener += listener;
        }

        public void RemoveListener(Action listener)
        {
            Listener -= listener;
        }
        
        protected void RemoveAllListeners()
        {
            Listener = null;
        }
    }

    public abstract class Observer<TParameter> : ObserverBase
    {
        private event Action<TParameter> Listener;

        public void Dispatch(TParameter value)
        {
            if (BeforeDispatch != null)
            {
                BeforeDispatch.Invoke(value);
            }

            if (Listener != null)
            {
                Listener.Invoke(value);
            }
        }
        
        public void AddListener(Action<TParameter> listener)
        {
            Listener += listener;
        }

        public void RemoveListener(Action<TParameter> listener)
        {
            Listener -= listener;
        }

        protected void RemoveAllListeners()
        {
            Listener = null;
        }
    }
}