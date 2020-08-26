using System;

namespace VoxCake.Framework
{
    public interface ITickable
    {
        event Action<float> Tick;
    }
}