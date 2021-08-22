namespace VoxCake.Framework
{
    public interface IViewBinder
    {
        IViewBindingSequence<TView> Bind<TView>() where TView : class;
        
        void Unbind<TBinding>();
    }
}