
using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

[CustomEditor(typeof(NormcoreBoolSync))]
public class NormcoreBoolSyncEditor : UnityEditor.Editor
{
    PropertyBinderEditor _propertyBinderEditor;
    private PropertyBinderEditor _floatPropertyBinderEditor; 
    private void OnEnable()
    {
        var finder = new PropertyFinder(serializedObject);
        
        _propertyBinderEditor = new PropertyBinderEditor(serializedObject.FindProperty("_propertyBinders"), "bool");
        _floatPropertyBinderEditor = new PropertyBinderEditor(serializedObject.FindProperty("_floatPropertyBinders"), "float");
        
    
    }

    public override bool RequiresConstantRepaint()
    {
        return EditorApplication.isPlaying && targets.Length == 1; 
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (targets.Length == 1 )
        {
            _propertyBinderEditor.ShowGUI();
        }
        
        if (targets.Length == 1 )
        {
            _floatPropertyBinderEditor.ShowGUI();
        }
    }
}
