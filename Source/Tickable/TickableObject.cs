using System;

namespace VoxCake.Framework
{
    internal class TickableObject : ITickable
    {
        public event Action<float> Tick;

        internal void InvokeTick(float dt)
        {
            Tick?.Invoke(dt);
        }
    }
}