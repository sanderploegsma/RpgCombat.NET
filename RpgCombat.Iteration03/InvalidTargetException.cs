using System;

namespace RpgCombat.Iteration03
{
    public class InvalidTargetException : InvalidOperationException
    {
        public InvalidTargetException(string message) : base(message)
        {
            
        }
    }
}