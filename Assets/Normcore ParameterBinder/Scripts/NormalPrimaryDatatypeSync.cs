using System;
using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;

public class NormalPrimaryDatatypeSync : RealtimeComponent
{
    [SerializeReference] [HideInInspector] private BoolPropertyBinder[] _boolPropertyBinders = null;

    public BoolPropertyBinder[] BoolPropertyBinders
    {
        get => (BoolPropertyBinder[]) _boolPropertyBinders.Clone();
        set => _boolPropertyBinders = value; 
    }

    [SerializeReference] [HideInInspector] private FloatPropertyBinder[] _floatPropertyBinders = null;

    public FloatPropertyBinder[] FloatPropertyBinders
    {
        get => (FloatPropertyBinder[]) _floatPropertyBinders.Clone();
        set => _floatPropertyBinders = value; 
    }
    
    [SerializeReference] [HideInInspector] private Vector3PropertyBinder[] _vector3PropertyBinders = null;

    public Vector3PropertyBinder[] Vector3PropertyBinders
    {
        get => (Vector3PropertyBinder[]) _vector3PropertyBinders.Clone();
        set => _vector3PropertyBinders = value; 
    }


    public float localFloatProperty;
    public bool localBoolProperty;
    public Vector3 localVector3Property; 


    private NormalPrimaryDatatypeModel _model;

    private NormalPrimaryDatatypeModel model
    {
        set
        {
            if (_model != null)
            {
                _model.boolPropertyDidChange -= ModelOnboolPropertyDidChange;
                _model.floatPropertyDidChange -= ModelOnfloatPropertyDidChange;
                _model.vector3PropertyDidChange -= ModelOnvector3PropertyDidChange;
            }

            _model = value;

            if (_model != null)
            {
                UpdateFloatProperty();
                UpdateBoolProperty();
                UpdateVector3Property(); 
                _model.boolPropertyDidChange += ModelOnboolPropertyDidChange;
                _model.floatPropertyDidChange += ModelOnfloatPropertyDidChange; 
                _model.vector3PropertyDidChange += ModelOnvector3PropertyDidChange;
            }
        }
    }

    private void UpdateVector3Property()
    {
        localVector3Property = _model.vector3Property;
        if (_vector3PropertyBinders != null)
        {
            foreach (var vector3PropertyBinder in _vector3PropertyBinders)
            {
                vector3PropertyBinder.vector3Property = localVector3Property;
            }
        }
    }

    private void ModelOnvector3PropertyDidChange(NormalPrimaryDatatypeModel normalPrimaryDatatypeModel, Vector3 value)
    {
        UpdateVector3Property();
    }

    private void UpdateBoolProperty()
    {
        localBoolProperty = _model.boolProperty;
        if (_boolPropertyBinders != null)
        {
            foreach (var boolPropertyBinder in _boolPropertyBinders)
            {
                boolPropertyBinder.boolProperty = localBoolProperty; 
            }
        }
    }

    private void UpdateFloatProperty()
    {
        localFloatProperty = _model.floatProperty;
        if (_floatPropertyBinders != null)
        {
            foreach (var floatPropertyBinder in _floatPropertyBinders)
            {
                floatPropertyBinder.floatProperty = localFloatProperty;
            }
        }
    }


    private void ModelOnfloatPropertyDidChange(NormalPrimaryDatatypeModel normalPrimaryDatatypeModel, float value)
    {
       UpdateFloatProperty();
    }

    private void ModelOnboolPropertyDidChange(NormalPrimaryDatatypeModel normalPrimaryDatatypeModel, bool value)
    {
        UpdateBoolProperty();
    }
    

    // Update is called once per frame
    void Update()
    {
        if(_model == null)
            return;

        if (_boolPropertyBinders != null)
        {
            foreach (var boolPropertyBinder in _boolPropertyBinders)
            {
                if (boolPropertyBinder.boolProperty != localBoolProperty)
                {
                    localBoolProperty = boolPropertyBinder.boolProperty;
                    _model.boolProperty = localBoolProperty; 
                }

               
            }
        }

        if (_floatPropertyBinders != null)
        {
            foreach (var floatPropertyBinder in _floatPropertyBinders)
            {
                if (Math.Abs(floatPropertyBinder.floatProperty - localFloatProperty) > 0.0f)
                {
                    localFloatProperty = floatPropertyBinder.floatProperty;
                    _model.floatProperty = localFloatProperty; 
                }
            }
        }
        
        if (_vector3PropertyBinders != null)
        {
            foreach (var vector3PropertyBinder in _vector3PropertyBinders)
            {
                if (vector3PropertyBinder.vector3Property != localVector3Property)
                {
                    localVector3Property = vector3PropertyBinder.vector3Property;
                    _model.vector3Property = localVector3Property; 
                }
            }
        }
    }
}
