using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.UnityMVVM
{
    public class BoolToInverseAdapter : PropertyAdapter<bool>
    {
        public override object Convert(object target)
        {
            bool targetAsBool = (bool) target;
            
            return !targetAsBool;
        }
    }
}
