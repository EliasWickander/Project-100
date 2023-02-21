using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.UnityMVVM
{
    public abstract class PropertyAdapter
    {
        public abstract object Convert(object target);
    }
    public abstract class PropertyAdapter<T> : PropertyAdapter
    {
        
    }
}
