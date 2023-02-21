using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.UnityMVVM
{
    public class FloatToStringAdapter : PropertyAdapter<float>
    {
        public override object Convert(object target)
        {
            float targetAsFloat = (float) target;
            
            return targetAsFloat.ToString();
        }
    }
}
