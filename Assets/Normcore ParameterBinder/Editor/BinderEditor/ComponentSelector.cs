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


sealed class ComponentSelector
{
    #region Static members

    // Return a selector instance for a given target.
    public static ComponentSelector GetInstance(SerializedProperty spTarget)
    {
        var component = spTarget.objectReferenceValue as Component;

        // Special case: The target is not specified.
        if (component == null) return _nullInstance;

        var gameObject = component.gameObject;

        // Try getting from the dictionary.
        ComponentSelector selector;
        if (_instances.TryGetValue(gameObject, out selector))
            return selector;

        // New instance
        selector = new ComponentSelector(gameObject);
        _instances[gameObject] = selector;
        return selector;
    }

    // Clear the cache contents.
    // It's recommended to invoke when the inspector is initiated.
    public static void InvalidateCache() => _instances.Clear();

    static Dictionary<GameObject, ComponentSelector> _instances
      = new Dictionary<GameObject, ComponentSelector>();

    static ComponentSelector _nullInstance = new ComponentSelector(null);

    #endregion

    #region Private constructor

    ComponentSelector(GameObject gameObject)
      => _candidates = gameObject?.GetComponents<Component>()
         .Select(c => c.GetType().Name).ToArray();

    string[] _candidates;

    #endregion

    #region GUI implementation

    public bool ShowGUI(SerializedProperty spTarget)
    {
        if (_candidates == null) return false;

        var component = (Component)spTarget.objectReferenceValue;
        var gameObject = component.gameObject;

        // Current selection
        var index = Array.IndexOf(_candidates, component.GetType().Name);

        // Component selection drop down
        EditorGUI.BeginChangeCheck();
        index = EditorGUILayout.Popup("Component", index, _candidates);
        if (EditorGUI.EndChangeCheck())
            spTarget.objectReferenceValue =
              gameObject.GetComponent(_candidates[index]);

        return true;
    }

    #endregion
}

