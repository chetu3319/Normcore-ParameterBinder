using Normal.Realtime; 
using UnityEngine;
using UnityEngine.Events;

public class NormalFloatSync : RealtimeComponent
{
    
    [SerializeReference] [HideInInspector] private FloatPropertyBinder[] _floatPropertyBinders = null;

    public FloatPropertyBinder[] FloatPropertyBinders
    {
        get => (FloatPropertyBinder[])_floatPropertyBinders.Clone();
        set => _floatPropertyBinders = value;
    }
    
    public float floatValue;

    
    private NormcoreFloatModel _model;

    private NormcoreFloatModel model
    {
        set
        {
            if (_model != null)
            {
                _model.floatPropertyDidChange -= ModelOnfloatPropertyDidChange;
            }

            _model = value;

            if (_model != null)
            {
                UpdateFloatValue();
                _model.floatPropertyDidChange += ModelOnfloatPropertyDidChange; 
            }
        }
    }

    private void UpdateFloatValue()
    {
       Debug.Log("Float: Updating local");

       floatValue = _model.floatProperty;
       if (_floatPropertyBinders != null)
       {
           foreach (var floatPropertyBinder in _floatPropertyBinders)
           {
               floatPropertyBinder.floatProperty = floatValue;
           }
       }

    }

    public void updateFValue(float val)
    {
        floatValue = val;
        _model.floatProperty = floatValue; 
    }

    private void ModelOnfloatPropertyDidChange(NormcoreFloatModel normcoreFloatModel, float value)
    {
        UpdateFloatValue();
    }

    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        if (_floatPropertyBinders != null)        
        {
            foreach (var floatPropertyBinder in _floatPropertyBinders)
            {
                if (floatPropertyBinder.floatProperty != floatValue)
                {
                    Debug.Log("Float: Updating Sever");
                    floatValue = floatPropertyBinder.floatProperty;
                    _model.floatProperty = floatValue;
                }
            }
        }
    }
}
