using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NormalPrimaryDatatypeSync))]
public class NormalPrimaryDatatypeSyncEditor : UnityEditor.Editor
{
    private PropertyBinderEditor _boolPropertyBinderEditor;
    private PropertyBinderEditor _floatPropertyBinderEditor;
    private PropertyBinderEditor _vector3PropertyBinderEditor; 

    private void OnEnable()
    {
        var finder = new PropertyFinder(serializedObject);
        
        _boolPropertyBinderEditor = new PropertyBinderEditor(serializedObject.FindProperty("_boolPropertyBinders"), "bool");
        _floatPropertyBinderEditor = new PropertyBinderEditor(serializedObject.FindProperty("_floatPropertyBinders"),"float");
        _vector3PropertyBinderEditor = new PropertyBinderEditor(serializedObject.FindProperty("_vector3PropertyBinders"), "Vector3");
    }

    public override bool RequiresConstantRepaint()
    {
        return EditorApplication.isPlaying && targets.Length == 1;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (targets.Length == 1)
        {
            _boolPropertyBinderEditor.ShowGUI();
            _floatPropertyBinderEditor.ShowGUI();
            _vector3PropertyBinderEditor.ShowGUI();
        }
    }
}
