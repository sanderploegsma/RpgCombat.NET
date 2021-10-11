using System;

namespace RpgCombat04
{
    public class InvalidTargetException : InvalidOperationException
    {
        public InvalidTargetException(string message) : base(message)
        {
            
        }
    }
}