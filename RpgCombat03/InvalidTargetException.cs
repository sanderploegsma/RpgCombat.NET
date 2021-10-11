using System;

namespace RpgCombat03
{
    public class InvalidTargetException : InvalidOperationException
    {
        public InvalidTargetException(string message) : base(message)
        {
            
        }
    }
}