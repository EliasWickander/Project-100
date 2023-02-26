using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Mirror;
using UnityEngine;

namespace Util.UnityMVVM
{
    public class ViewModelNetworkBehaviour : NetworkBehaviour, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        protected void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }
    }
}
