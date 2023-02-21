using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.UnityMVVM
{
    public abstract class PropertyBinding : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            Connect();
        }

        protected virtual void OnDisable()
        {
            Disconnect();
        }

        protected abstract void Connect();
        protected abstract void Disconnect();
        
        protected void ParseMemberReferenceData(string reference, out string componentName, out string memberName)
        {
            string[] parts = reference.Split('.');
            
            componentName = parts[^2];
            memberName = parts[^1];
        }
    }
}
