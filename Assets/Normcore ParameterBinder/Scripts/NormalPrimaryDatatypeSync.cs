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


    public float localFloatProperty;
    public bool localBoolProperty;


    private NormalPrimaryDatatypeModel _model;

    private NormalPrimaryDatatypeModel model
    {
        set
        {
            if (_model != null)
            {
                _model.boolPropertyDidChange -= ModelOnboolPropertyDidChange;
                _model.floatPropertyDidChange -= ModelOnfloatPropertyDidChange;
            }

            _model = value;

            if (_model != null)
            {
                UpdateFloatProperty();
                UpdateBoolProperty();
                _model.boolPropertyDidChange += ModelOnboolPropertyDidChange;
                _model.floatPropertyDidChange += ModelOnfloatPropertyDidChange; 
            }
        }
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
    }
}
