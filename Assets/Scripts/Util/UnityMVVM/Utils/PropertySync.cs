using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.UnityMVVM
{
    public enum PropertySyncMode
    {
        SourceToDest,
        DestToSource
    }
    
    public class PropertySync
    {
        private PropertyEndPoint m_source;
        private PropertyEndPoint m_dest;
        
        public PropertySync(PropertyEndPoint source, PropertyEndPoint dest)
        {
            m_source = source;
            m_dest = dest;
        }

        public void Sync(PropertySyncMode mode)
        {
            switch (mode)
            {
                case PropertySyncMode.SourceToDest:
                {
                    SyncSourceToDest();
                    break;
                }
                case PropertySyncMode.DestToSource:
                {
                    SyncDestToSource();
                    break;
                }
            }
        }

        private void SyncSourceToDest()
        {
            object sourceValue = m_source.GetValue();
            
            m_dest.SetValue(sourceValue);
        }

        private void SyncDestToSource()
        {
            object destValue = m_dest.GetValue();
            
            m_source.SetValue(destValue);
        }
    }
}
