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

using Normal.Realtime;
using UnityEngine;

namespace chetu3319.ParameterBinder
{
    [RequireComponent(typeof(RealtimeView))]
    public class RealtimeFloatSync : RealtimeComponent<RealtimeFloatModel>
    {
        #region Property Binders

        [SerializeReference] [HideInInspector] private FloatPropertyBinder[] _floatPropertyBinders = null;

        public FloatPropertyBinder[] FloatPropertyBinders
        {
            get => (FloatPropertyBinder[]) _floatPropertyBinders.Clone();
            set => _floatPropertyBinders = value;
        }

        #endregion

        // Local Variable which will be synced with the network. 
        // Property binders will subscribe to this value to be in sync. 
        [HideInInspector] public float localFloatValue;

        #region Normcore Realtime Logic

        private void UpdateFloatProperty()
        {
            localFloatValue = this.model.floatProperty;
            if (_floatPropertyBinders != null)
            {
                foreach (var floatPropertyBinder in _floatPropertyBinders)
                {
                    floatPropertyBinder.floatProperty = localFloatValue;
                }
            }
        }

        private void ModelOnfloatPropertyDidChange(RealtimeFloatModel normcoreFloatModel, float value)
        {
            UpdateFloatProperty();
        }

        protected override void OnRealtimeModelReplaced(RealtimeFloatModel previousModel,
            RealtimeFloatModel currentModel)
        {
            if (previousModel != null)
            {
                previousModel.floatPropertyDidChange -= ModelOnfloatPropertyDidChange;
            }

            if (currentModel != null)
            {
                if (!currentModel.isFreshModel)
                {
                    UpdateFloatProperty();
                }

                currentModel.floatPropertyDidChange += ModelOnfloatPropertyDidChange;
            }
        }

        #endregion

        #region MonoBehaviour Functions

        private void Awake()
        {
            if (_floatPropertyBinders != null)
            {
                localFloatValue = _floatPropertyBinders[0].floatProperty;
                foreach (var floatPropertyBinder in _floatPropertyBinders)
                {
                    floatPropertyBinder.floatProperty = localFloatValue;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_floatPropertyBinders != null)
            {
                foreach (var floatPropertyBinder in _floatPropertyBinders)
                {
                    if (floatPropertyBinder.floatProperty != localFloatValue ||
                        (this.model != null && this.model.floatProperty != localFloatValue))
                    {
                        localFloatValue = floatPropertyBinder.floatProperty;
                        if (this.model != null)
                        {
                            this.model.floatProperty = localFloatValue;
                        }
                    }
                }
            }
        }

        #endregion
    }
}