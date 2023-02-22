﻿using System;

namespace Util.UnityMVVM.Exceptions
{
    /// <summary>
    /// Thrown when there is an error binding to a UnityEvent.
    /// </summary>
    public class InvalidEventException : Exception
    {
        public InvalidEventException(string message)
            : base(message)
        {
        }

        public InvalidEventException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
