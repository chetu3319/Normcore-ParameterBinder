using System;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection; 


//
// Property binder classes used for driving properties of external objects
// by audio level
//

// Property binder base class
[System.Serializable]
public abstract class BoolPropertyBinder
{
    // Enable switch
    public bool Enabled = true;

    // Audio level property (setter only)
    public bool boolProperty { get
        {
            return OnGetLevel(); 
        }
    set { if (Enabled) OnSetLevel(value); } }

    // Binder implementation
    protected abstract void OnSetLevel(bool level);
    protected abstract bool OnGetLevel(); 
}

// Generic intermediate implementation
public abstract class GenericBoolPropertyBinder<T> : BoolPropertyBinder
{
    // Serialized target property information
    public Component Target;
    public string PropertyName;

    // This field in only used in Editor to determine the target property
    // type. Don't modify it after instantiation.
    [SerializeField, HideInInspector]
    string _propertyType = typeof(T).AssemblyQualifiedName;

    // Target property setter
    protected T TargetProperty
    {
        get
        {
            return (T)GetProperty(Target, PropertyName);
        }
        set => SetTargetProperty(value);
    }

    UnityAction<T> _setterCache;

    private static object GetProperty(Component inObj, string fieldName)
    {
        object ret = null;
        Type myObj = inObj.GetType();

        PropertyInfo info = myObj.GetProperty(fieldName);
        
       // FieldInfo info = inObj.GetType().GetField(fieldName);
        if (info != null)
            ret = info.GetValue(inObj);
        return ret;
    }
    void SetTargetProperty(T value)
    {
        if (_setterCache == null)
        {
            if (Target == null) return;
            if (string.IsNullOrEmpty(PropertyName)) return;
          
            _setterCache
              = (UnityAction<T>)System.Delegate.CreateDelegate
                (typeof(UnityAction<T>), Target, "set_" + PropertyName);
        }
        _setterCache(value);
    }
}

//Binder for Boolean Values
public sealed class BoolValuePropertyBinder : GenericBoolPropertyBinder<bool>
{
    // public bool Value0 = false;
    // public bool Value1 = true;

   
    protected override void OnSetLevel(bool level)
    {
       
        TargetProperty = level;
    }

    protected override bool OnGetLevel()
    {
        return TargetProperty; 
    }
}




[System.Serializable]
public abstract class FloatPropertyBinder
{
    public bool Enabled = true;

    public float floatProperty
    {
        get { return OnGetLevel(); }
        set
        {
            if (Enabled) OnSetLevel(value);
        }
    }

    protected abstract void OnSetLevel(float level);
    protected abstract float OnGetLevel(); 
}

public abstract class GenericFloatPropertyBinder<T> : FloatPropertyBinder
{
    // Serialized target property information
    public Component Target;
    public string PropertyName;

    // This field in only used in Editor to determine the target property
    // type. Don't modify it after instantiation.
    [SerializeField, HideInInspector]
    string _propertyType = typeof(T).AssemblyQualifiedName;

    // Target property setter
    protected T TargetProperty
    {
        get
        {
            return (T)GetProperty(Target, PropertyName);
        }
        set => SetTargetProperty(value);
    }

    UnityAction<T> _setterCache;

    private static object GetProperty(Component inObj, string fieldName)
    {
        object ret = null;
        Type myObj = inObj.GetType();

        PropertyInfo info = myObj.GetProperty(fieldName);
        
        // FieldInfo info = inObj.GetType().GetField(fieldName);
        if (info != null)
            ret = info.GetValue(inObj);
        return ret;
    }
    void SetTargetProperty(T value)
    {
        if (_setterCache == null)
        {
            if (Target == null) return;
            if (string.IsNullOrEmpty(PropertyName)) return;
          
            _setterCache
                = (UnityAction<T>)System.Delegate.CreateDelegate
                    (typeof(UnityAction<T>), Target, "set_" + PropertyName);
        }
        _setterCache(value);
    }
}

public sealed class FloatValuePropertyBinder : GenericFloatPropertyBinder<float>
{
    // public float Value0 = 0;
    // public float Value1 = 1.0f;

   
    protected override void OnSetLevel(float level)
    {
       
        TargetProperty = level;
    }

    protected override float OnGetLevel()
    {
        return TargetProperty; 
    }
}


[System.Serializable]
public abstract class Vector3PropertyBinder
{
    public bool Enabled = true;

    public Vector3 vector3Property
    {
        get { return OnGetProperty(); }
        set
        {
            if (Enabled) OnSetProperty(value);
        }
    }

    protected abstract void OnSetProperty(Vector3 value);
    protected abstract Vector3 OnGetProperty(); 
}

public abstract class GenericVector3PropertyBinder<T> : Vector3PropertyBinder
{
    // Serialized target property information
    public Component Target;
    public string PropertyName;

    // This field in only used in Editor to determine the target property
    // type. Don't modify it after instantiation.
    [SerializeField, HideInInspector]
    string _propertyType = typeof(T).AssemblyQualifiedName;

    // Target property setter
    protected T TargetProperty
    {
        get
        {
            return (T)GetProperty(Target, PropertyName);
        }
        set => SetTargetProperty(value);
    }

    UnityAction<T> _setterCache;

    private static object GetProperty(Component inObj, string fieldName)
    {
        object ret = null;
        Type myObj = inObj.GetType();

        PropertyInfo info = myObj.GetProperty(fieldName);
        
        // FieldInfo info = inObj.GetType().GetField(fieldName);
        if (info != null)
            ret = info.GetValue(inObj);
        return ret;
    }
    void SetTargetProperty(T value)
    {
        if (_setterCache == null)
        {
            if (Target == null) return;
            if (string.IsNullOrEmpty(PropertyName)) return;
          
            _setterCache
                = (UnityAction<T>)System.Delegate.CreateDelegate
                    (typeof(UnityAction<T>), Target, "set_" + PropertyName);
        }
        _setterCache(value);
    }
}

public sealed class Vector3ValuePropertyBinder : GenericVector3PropertyBinder<Vector3>
{
    // public float Value0 = 0;
    // public float Value1 = 1.0f;

   
    protected override void OnSetProperty(Vector3 value)
    {
       
        TargetProperty = value;
    }

    protected override Vector3 OnGetProperty()
    {
        return TargetProperty; 
    }
}
