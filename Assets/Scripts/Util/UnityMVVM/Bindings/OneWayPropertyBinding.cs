using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Component = UnityEngine.Component;

namespace Util.UnityMVVM
{
    public class OneWayPropertyBinding : PropertyBinding
    {
        public string ViewModelPropertyName
        {
            get
            {
                return m_viewModelPropertyName;
            }
            set
            {
                m_viewModelPropertyName = value;
            }
        }
        
        public string m_viewModelPropertyName = "";
        
        public string ViewPropertyName
        {
            get
            {
                return m_viewPropertyName;
            }
            set
            {
                m_viewPropertyName = value;
            }
        }
        public string m_viewPropertyName = "";

        public string ViewPropertyType
        {
            get
            {
                return m_viewPropertyType;
            }
            set
            {
                m_viewPropertyType = value;
            }
        }
        
        public string m_viewPropertyType;

        public string ViewPropertyAdapter
        {
            get
            {
                return m_viewPropertyAdapter;
            }
            set
            {
                m_viewPropertyAdapter = value;
            }
        }

        public string m_viewPropertyAdapter = "";
        
        private PropertyObserver m_viewModelPropertyObserver;

        protected override void Connect()
        {
            ParseMemberReferenceData(ViewPropertyName, out string viewComponent, out string viewProperty);
            ParseMemberReferenceData(ViewModelPropertyName, out string viewModelComponent, out string viewModelProperty);
            
            Component view = GetComponent(viewComponent);
            Component viewModel = GetComponent(viewModelComponent);

            PropertyAdapter adapter = CreateAdapter(m_viewPropertyAdapter);
            
            PropertyEndPoint viewPropertyEndPoint = new PropertyEndPoint(view, viewProperty, adapter);
            PropertyEndPoint viewModelPropertyEndPoint = new PropertyEndPoint(viewModel, viewModelProperty, adapter);

            PropertySync propertySync = new PropertySync(viewModelPropertyEndPoint, viewPropertyEndPoint);

            if(viewModel is INotifyPropertyChanged notifyPropertyChanged) 
                m_viewModelPropertyObserver = new PropertyObserver(notifyPropertyChanged, () => propertySync.Sync(PropertySyncMode.SourceToDest));
        }

        protected override void Disconnect()
        {
            if (m_viewModelPropertyObserver != null)
            {
                m_viewModelPropertyObserver.Dispose();
                m_viewModelPropertyObserver = null;   
            }
        }

        private PropertyAdapter CreateAdapter(string adapterName)
        {
            if (string.IsNullOrEmpty(adapterName))
                return null;
            
            Type adapterType = MVVMHelper.FindAdapter(adapterName);

            return Activator.CreateInstance(adapterType) as PropertyAdapter;
        }
    }
}
