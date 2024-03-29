﻿using System;

namespace Util.UnityMVVM.Exceptions
{
    /// <summary>
    /// Thrown when the necessary template for a TemplateBinding or CollectionBinding 
    /// could not be found.
    /// </summary>
    public class TemplateNotFoundException : Exception
    {
        public TemplateNotFoundException(string message)
            : base(message)
        {
        }
    }
}
