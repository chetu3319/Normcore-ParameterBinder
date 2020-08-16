using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Normal.ParameterBinder.Integer
{
    public class DebugScript : MonoBehaviour
    {
        [SerializeField] [Range(0, 100)] private int _int; 
        public int intProperty
        {
            get => _int;
            set => _int = value; 
        }
    }
}

