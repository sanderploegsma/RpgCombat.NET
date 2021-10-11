using System;

namespace RpgCombat.Iteration05
{
    public class InvalidTargetException : InvalidOperationException
    {
        public InvalidTargetException(string message) : base(message)
        {
            
        }
    }
}