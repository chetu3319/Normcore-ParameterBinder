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

// Simple string label with GUIContent
struct Label
{
    GUIContent _guiContent;

    public static implicit operator GUIContent(Label label)
      => label._guiContent;

    public static implicit operator Label(string text)
      => new Label { _guiContent = new GUIContent(text) };
}

// Utilities for finding serialized properties
struct PropertyFinder
{
    SerializedObject _so;

    public PropertyFinder(SerializedObject so)
      => _so = so;

    public SerializedProperty this[string name]
      => _so.FindProperty(name);
}

struct RelativePropertyFinder
{
    SerializedProperty _sp;

    public RelativePropertyFinder(SerializedProperty sp)
      => _sp = sp;

    public SerializedProperty this[string name]
      => _sp.FindPropertyRelative(name);
}

static class PropertyBinderNameUtil
{
    public static string Shorten(SerializedProperty prop)
      => ObjectNames.NicifyVariableName(
           prop.managedReferenceFullTypename
           .Replace("Assembly-CSharp chetu3319.ParameterBinder.", ""));
}

static class PropertyBinderTypeLabel<T>
{
    static public GUIContent Content => _gui;
    static GUIContent _gui = new GUIContent
      (ObjectNames.NicifyVariableName(typeof(T).Name)
         .Replace("Property Binder", ""));
}

