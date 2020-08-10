
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
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;



sealed class PropertySelector
{
    #region Static members

    // Return a selector instance for a given type pair.
    public static PropertySelector GetInstance
      (SerializedProperty spTarget, SerializedProperty spPropertyType)
    {
        var key = spTarget.objectReferenceValue.GetType()
                  + spPropertyType.stringValue;

        // Try getting from the dictionary.
        PropertySelector selector;
        if (_instances.TryGetValue(key, out selector)) return selector;

        // New instance
        selector = new PropertySelector(spTarget, spPropertyType);
        _instances[key] = selector;
        return selector;
    }

    static Dictionary<string, PropertySelector> _instances
      = new Dictionary<string, PropertySelector>();

    #endregion

    #region Private constructor

    PropertySelector
      (SerializedProperty spTarget, SerializedProperty spPropertyType)
    {
        // Determine the target property type using reflection.
        _propertyType = Type.GetType(spPropertyType.stringValue);

        // Property name candidates query
        _candidates = spTarget.objectReferenceValue.GetType()
          .GetProperties(BindingFlags.Public | BindingFlags.Instance)
          .Where(prop => prop.PropertyType == _propertyType)
          .Select(prop => prop.Name).ToArray();
    }

    Type _propertyType;
    string[] _candidates;

    #endregion

    #region GUI implementation

    public bool ShowGUI(SerializedProperty spPropertyName)
    {
        // Clear the selection and show a message if there is no candidate.
        if (_candidates.Length == 0)
        {
            EditorGUILayout.HelpBox
              ($"No {_propertyType.Name} property found.",
               MessageType.None);
            spPropertyName.stringValue = null;
            return false;
        }

        // Index of the current selection
        var index = Array.IndexOf(_candidates, spPropertyName.stringValue);

        // Drop down list
        EditorGUI.BeginChangeCheck();
        index = EditorGUILayout.Popup("Property", index, _candidates);
        if (EditorGUI.EndChangeCheck())
            spPropertyName.stringValue = _candidates[index];

        // Return true only when the selection is valid.
        return index >= 0;
    }

    #endregion
}

