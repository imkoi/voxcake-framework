namespace VoxCake.Framework
{
    public abstract class Command<TObserver> where TObserver : Observer
    {
        private readonly TObserver _observer;
        
        protected Command(TObserver observer)
        {
            _observer = observer;
            observer.AddListener(Execute);
        }

        protected abstract void Execute();
    }
    
    public abstract class Command<TObserver, TParameter> where TObserver : Observer<TParameter>
    {
        private readonly TObserver _observer;
        
        protected Command(TObserver observer)
        {
            _observer = observer;
            observer.AddListener(Execute);
        }

        protected abstract void Execute(TParameter parameter);
    }
}