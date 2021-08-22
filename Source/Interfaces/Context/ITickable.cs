using System;

namespace VoxCake.Framework
{
    public interface ITickable
    {
        event Action<float> Tick;
        event Action<float> FixedTick;
        event Action<float> LateTick;
    }
}