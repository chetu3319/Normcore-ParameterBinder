#region License
//------------------------------------------------------------------------------ -
// Normcore-ParameterBinder
// https://github.com/chetu3319/Normcore-ParameterBinder
//------------------------------------------------------------------------------ -
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

namespace chetu3319.ParameterBinder
{
    [CustomEditor(typeof(RealtimeVector3Sync))]
    public class RealtimeVector3SyncEditor : Editor
    {
        private PropertyBinderEditor _vector3PropertyBinderEditor;

        private void OnEnable()
        {
            var finder = new PropertyFinder(serializedObject);

            _vector3PropertyBinderEditor =
                new PropertyBinderEditor(serializedObject.FindProperty("_vector3PropertyBinders"), "Vector3");

        }

        public override bool RequiresConstantRepaint()
        {
            return EditorApplication.isPlaying && targets.Length == 1;
        }

        public override void OnInspectorGUI()
        {
            RealtimeVector3Sync data = (RealtimeVector3Sync) target;
            base.OnInspectorGUI();
            GUILayout.Label("Local Vector3 Value: " + data.localVector3Value);
            if (targets.Length == 1)
            {
                _vector3PropertyBinderEditor.ShowGUI();
            }
        }
    }
}

