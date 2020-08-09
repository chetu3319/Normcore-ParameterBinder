using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 

[CustomEditor(typeof(NormalFloatSync))]
public class NormcoreFloatSyncEditor : UnityEditor.Editor
{
    private PropertyBinderEditor _floatPropertyBinderEditor;

    private void OnEnable()
    {
        var finder = new PropertyFinder(serializedObject);
        
        _floatPropertyBinderEditor = new PropertyBinderEditor(serializedObject.FindProperty("_floatPropertyBinders" ),"float");
        
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
            _floatPropertyBinderEditor.ShowGUI();
        }
    }
}

