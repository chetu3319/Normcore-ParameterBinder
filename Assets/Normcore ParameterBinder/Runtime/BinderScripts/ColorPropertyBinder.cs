#region License

//------------------------------------------------------------------------------ -
// Normcore-ParameterBinder
// https://github.com/chetu3319/Normcore-ParameterBinder
//------------------------------------------------------------------------------ -
// Original Author: Keijiro Takahashi
// Gituhb Repo: https://github.com/keijiro/Lasp/blob/v2/Packages/jp.keijiro.lasp/Runtime/PropertyBinder.cs
//------------------------------------------------------------------------------ -
//
// MIT License
//
// Copyright (c) 2020 Chaitanya Shah
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//------------------------------------------------------------------------------ -

#endregion

using System;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;

namespace chetu3319.ParameterBinder
{
    //
// Property binder classes used for driving properties of external objects
//

// Property binder base class

    [System.Serializable]
    public abstract class ColorPropertyBinder
    {
        public bool Enabled = true;

        public Color colorProperty
        {
            get { return OnGetProperty(); }
            set
            {
                if (Enabled) OnSetProperty(value);
            }
        }

        protected abstract void OnSetProperty(Color value);
        protected abstract Color OnGetProperty();
    }

    public abstract class GenericColorPropertyBinder<T> : ColorPropertyBinder
    {
        // Serialized target property information
        public Component Target;
        public string PropertyName;

        // This field in only used in Editor to determine the target property
        // type. Don't modify it after instantiation.
        [SerializeField, HideInInspector] string _propertyType = typeof(T).AssemblyQualifiedName;

        // Target property setter
        protected T TargetProperty
        {
            get { return (T) GetProperty(Target, PropertyName); }
            set => SetTargetProperty(value);
        }

        UnityAction<T> _setterCache;

        private static object GetProperty(Component inObj, string fieldName)
        {
            object ret = null;
            Type myObj = inObj.GetType();
            PropertyInfo info = myObj.GetProperty(fieldName);

            // FieldInfo info = inObj.GetType().GetField(fieldName);
            if (info != null) ret = info.GetValue(inObj);
            return ret;
        }

        void SetTargetProperty(T value)
        {
            if (_setterCache == null)
            {
                if (Target == null) return;
                if (string.IsNullOrEmpty(PropertyName)) return;
                _setterCache =
                    (UnityAction<T>) System.Delegate.CreateDelegate(typeof(UnityAction<T>), Target,
                        "set_" + PropertyName);
            }

            _setterCache(value);
        }
    }

    public sealed class ColorValuePropertyBinder : GenericColorPropertyBinder<Color>
    {
        protected override void OnSetProperty(Color value)
        {
            TargetProperty = value;
        }

        protected override Color OnGetProperty()
        {
            return TargetProperty;
        }
    }
}