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

    private void OnEnable()
    {
        var finder = new PropertyFinder(serializedObject);
        
        _boolPropertyBinderEditor = new PropertyBinderEditor(serializedObject.FindProperty("_boolPropertyBinders"), "bool");
        _floatPropertyBinderEditor = new PropertyBinderEditor(serializedObject.FindProperty("_floatPropertyBinders"),"float");
        
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
        }
    }
}
