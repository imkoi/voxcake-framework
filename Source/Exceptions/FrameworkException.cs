using System;

namespace VoxCake.Framework
{
    public class FrameworkException : Exception
    {
        public FrameworkException(string message) : base(message)
        {
            
        }

        public FrameworkException(string message, Exception exception) : base(message, exception)
        {
            
        }
    }
}