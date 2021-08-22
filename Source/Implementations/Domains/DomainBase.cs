namespace VoxCake.Framework
{
    public abstract class DomainBase
    {
        public virtual bool CanBeEnabled(IContext context)
        {
            return false;
        }
        
        public virtual void OnInstall(IContext context)
        {
            
        }

        public virtual void OnEnable(IContext context)
        {
            
        }
    }
}