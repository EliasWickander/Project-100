using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util.UnityMVVM
{
    public class ViewModelTest : ViewModelMonoBehaviour
    {
        public float m_timer;

        public float m_delayUntilChange = 2;

        private bool m_testBool = false;

        private PropertyChangedEventArgs m_testBoolProp = new PropertyChangedEventArgs(nameof(TestBool));
        
        [Binding]
        public bool TestBool
        {
            get
            {
                return m_testBool;
            }
            set
            {
                m_testBool = value;
                OnPropertyChanged(m_testBoolProp);
            }
        }

        private void Update()
        {
            if (m_timer < m_delayUntilChange)
            {
                m_timer += Time.deltaTime;
            }
            else
            {
                m_timer = 0;
                
                TestBool = !TestBool;
                
                Debug.Log(TestBool);
            }
        }
    }
}
