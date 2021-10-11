using System;

namespace RpgCombat.Iteration04
{
    public class InvalidTargetException : InvalidOperationException
    {
        public InvalidTargetException(string message) : base(message)
        {
            
        }
    }
}