using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chetu3319.ParameterBinder
{
    public class DebugScript : MonoBehaviour
    {
        [SerializeField] [Range(0, 100)] private int _int;

        public int intProperty
        {
            get => _int;
            set => _int = value;
        }


        [SerializeField] [Range(0.0f, 1.0f)] private float _float;

        public float floatProperty
        {
            get => _float;
            set => _float = value;
        }

        [SerializeField] private Color _color;

        public Color colorProperty
        {
            get => _color;
            set => _color = value;
        }

        [SerializeField] private string _string;

        public string stringProperty
        {
            get => _string;
            set => _string = value;
        }

        [SerializeField] private bool _bool;

        public bool booleanProperty
        {
            get => _bool;
            set => _bool = value;
        }

        [SerializeField] private Vector3 _vector3;

        public Vector3 vector3Property
        {
            get => _vector3;
            set => _vector3 = value;
        }
    }
}