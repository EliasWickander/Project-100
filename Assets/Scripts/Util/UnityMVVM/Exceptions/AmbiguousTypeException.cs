using System;

namespace Util.UnityMVVM.Exceptions
{
    /// <summary>
    /// Thrown when we are searching for a single type but multiple ones 
    /// match our query.
    /// </summary>
    public class AmbiguousTypeException : Exception
    {
        public AmbiguousTypeException(string message)
            : base(message)
        {
        }
    }
}
