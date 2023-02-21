using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Util.UnityMVVM
{
    public class PropertyObserver
    {
        private INotifyPropertyChanged m_subject;
        private Action m_onChangedEvent;
        
        public PropertyObserver(INotifyPropertyChanged subject, Action onChanged)
        {
            m_subject = subject;
            m_onChangedEvent = onChanged;

            subject.PropertyChanged += OnPropertyChanged;
        }

        public void Dispose()
        {
            m_subject.PropertyChanged -= OnPropertyChanged;
        }
        
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            m_onChangedEvent?.Invoke();
        }
    }
}
