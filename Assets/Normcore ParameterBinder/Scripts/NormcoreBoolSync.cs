
using Normal.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class NormcoreBoolSync : RealtimeComponent
{

    // Property binders
    [SerializeReference] [HideInInspector] BoolPropertyBinder[] _propertyBinders = null;
    
    
    public BoolPropertyBinder[] PropertyBinders
    {
        get => (BoolPropertyBinder[])_propertyBinders.Clone();
        set => _propertyBinders = value;
    }

    [SerializeReference] [HideInInspector] private FloatPropertyBinder[] _floatPropertyBinders = null;

    public FloatPropertyBinder[] FloatPropertyBinders
    {
        get => (FloatPropertyBinder[])_floatPropertyBinders.Clone();
        set => _floatPropertyBinders = value;
    }

    public float sliderValue; 
    public bool flagValue = false;

    //x public UnityEvent<bool> onValueChange; 

    private NormcoreBoolModel _model;

    private NormcoreBoolModel model {
        set
        {

            if (_model != null)
            {
                _model.propertyDidChange -= ModelOnpropertyDidChange;
            }

            _model = value;

            if (_model != null)
            {
                updateFlagValue();


                _model.propertyDidChange += ModelOnpropertyDidChange; 
            }
        }
    }
    

    private void updateFlagValue()
    {
        Debug.Log("Updating local");
        flagValue = _model.property;
        if (_propertyBinders != null)
        {
            foreach (var propertyBinder in _propertyBinders)
            {
                //Debug.Log(propertyBinder.boolProperty);
                propertyBinder.boolProperty = flagValue; 
               
            }
                    
        }
    }

    private void ModelOnpropertyDidChange(NormcoreBoolModel normcoreBoolModel, bool value)
    {
        updateFlagValue();
    }

    
    public void toggleValue()
    {
        flagValue = !flagValue; 
        Debug.Log("updating server");
        _model.property = flagValue; 
    }


    private void Update()
    {
        foreach (var propertyBinder in _propertyBinders)
        {
            if (propertyBinder.boolProperty != flagValue)
            {
                flagValue = propertyBinder.boolProperty;
                _model.property = flagValue; 

            }
        }
        
        
        if (_floatPropertyBinders != null)
        {
            foreach (var propertyBinder in _floatPropertyBinders)
            {
                //Debug.Log(propertyBinder.boolProperty);
                propertyBinder.floatProperty = sliderValue; 
               
            }
                    
        }
    }
}
