using System;

namespace VoxCake.Framework
{
    public class ObserverBase
    {
        internal Action<object> BeforeDispatch;
    }
}