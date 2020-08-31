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
    public class RealtimeIntSync : RealtimeComponent<RealtimeIntModel>
    {
        #region Property Binders

        [SerializeReference] [HideInInspector] private IntPropertyBinder[] _intPropertyBinders = null;

        public IntPropertyBinder[] IntPropertyBinders
        {
            get => (IntPropertyBinder[]) _intPropertyBinders.Clone();
            set => _intPropertyBinders = value;
        }

        #endregion

        // Local Variable which will be synced with the network. 
        // Property binders will subscribe to this value to be in sync. 
        [HideInInspector] public int localIntValue;

        #region Normcore Realtime Logic

        private void ModelOnintPropertyDidChange(RealtimeIntModel normcoreFloatModel, int value)
        {
            UpdateIntProperty();
        }

        private void UpdateIntProperty()
        {
            localIntValue = model.intProperty;
            if (_intPropertyBinders != null)
            {
                foreach (var floatPropertyBinder in _intPropertyBinders)
                {
                    floatPropertyBinder.floatProperty = localIntValue;
                }
            }
        }

        protected override void OnRealtimeModelReplaced(RealtimeIntModel previousModel, RealtimeIntModel currentModel)
        {
            if (previousModel != null)
            {
                previousModel.intPropertyDidChange -= ModelOnintPropertyDidChange;
            }

            if (currentModel != null)
            {
                if (!currentModel.isFreshModel)
                {
                    UpdateIntProperty();
                }

                currentModel.intPropertyDidChange += ModelOnintPropertyDidChange;
            }
        }

        #endregion

        #region MonoBehaviour Functions

        private void Awake()
        {
            // Fetching the value of first binder and syncing it with local as well as other binders. 
            if (_intPropertyBinders != null)
            {
                localIntValue = _intPropertyBinders[0].floatProperty;
                foreach (var floatPropertyBinder in _intPropertyBinders)
                {
                    floatPropertyBinder.floatProperty = localIntValue;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_intPropertyBinders != null)
            {
                foreach (var floatPropertyBinder in _intPropertyBinders)
                {
                    if (floatPropertyBinder.Enabled && (floatPropertyBinder.floatProperty != localIntValue ||
                                                        (model != null && model.intProperty != localIntValue)))
                    {
                        localIntValue = floatPropertyBinder.floatProperty;
                        if (model != null)
                        {
                            model.intProperty = localIntValue;
                        }
                    }
                }
            }
        }

        #endregion
    }
}