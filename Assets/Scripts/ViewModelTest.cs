using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util.UnityMVVM
{
    [Binding]
    public class ViewModelTest2 : ViewModel
    {
        private string m_testString;
        private PropertyChangedEventArgs m_testStringProp = new PropertyChangedEventArgs(nameof(TestString));

        [Binding]
        public string TestString
        {
            get
            {
                return m_testString;
            }
            set
            {
                m_testString = value;
                OnPropertyChanged(m_testStringProp);
            }
        }
    }
    [Binding]
    public class ViewModelTest : ViewModelMonoBehaviour
    {
        public float m_timer;

        public float m_delayUntilChange = 2;

        private bool m_testBool = false;

        private PropertyChangedEventArgs m_testBoolProp = new PropertyChangedEventArgs(nameof(TestBool));

        private PropertyChangedEventArgs m_testModelProp = new PropertyChangedEventArgs(nameof(TestModel));
        private ViewModelTest2 m_testModel;
        
        [Binding]
        public ViewModelTest2 TestModel
        {
            get
            {
                return m_testModel;
            }
            set
            {
                m_testModel = value;
                OnPropertyChanged(m_testModelProp);
            }
        }
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
