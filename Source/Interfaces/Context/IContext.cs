using System;

namespace VoxCake.Framework
{
    public interface IContext
    {
        IBasicBinder BasicBinder { get; }
        IViewBinder ViewBinder { get; }
        ICommandBinder CommandBinder { get; }
        IDomainManager DomainManager { get; }

        TInstance GetInstance<TInstance>();
        object GetInstance(Type instanceType);
    }
}