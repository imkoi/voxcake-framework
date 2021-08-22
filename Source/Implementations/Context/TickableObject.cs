using System;

namespace VoxCake.Framework.Implementations
{
    internal class TickableObject : ITickable
    {
        public event Action<float> Tick;
        public event Action<float> FixedTick;
        public event Action<float> LateTick;
 
        internal void InvokeTick(float dt)
        {
            if (Tick != null)
            {
                Tick.Invoke(dt);
            }
        }

        internal void InvokeFixedTick(float dt)
        {
            if (FixedTick != null)
            {
                FixedTick.Invoke(dt);
            }
        }

        internal void InvokeLateTick(float dt)
        {
            if (LateTick != null)
            {
                LateTick.Invoke(dt);
            }
        }
    }
}