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
    public class RealtimeBoolSync : RealtimeComponent<RealtimeBoolModel>
    {
        #region Property Binders

        [SerializeReference] [HideInInspector] BoolPropertyBinder[] _boolPropertyBinders = null;

        public BoolPropertyBinder[] PropertyBinders
        {
            get => (BoolPropertyBinder[]) _boolPropertyBinders.Clone();
            set => _boolPropertyBinders = value;
        }

        #endregion

        // Local Variable which will be synced with the network. 
        // Property binders will subscribe to this value to be in sync. 
        [HideInInspector] public bool localBoolValue;

        #region Normcore Realtime Logic

        protected override void OnRealtimeModelReplaced(RealtimeBoolModel previousModel, RealtimeBoolModel currentModel)
        {
            if (previousModel != null)
            {
                previousModel.boolPropertyDidChange -= ModelOnboolPropertyDidChange;
            }

            if (currentModel != null)
            {
                if (!currentModel.isFreshModel)
                {
                    UpdateBoolProperty();
                }

                currentModel.boolPropertyDidChange += ModelOnboolPropertyDidChange;
            }
        }

        private void UpdateBoolProperty()
        {
            localBoolValue = this.model.boolProperty;
            if (_boolPropertyBinders != null)
            {
                foreach (var propertyBinder in _boolPropertyBinders)
                {
                    propertyBinder.boolProperty = localBoolValue;
                }
            }
        }

        private void ModelOnboolPropertyDidChange(RealtimeBoolModel normcoreBoolModel, bool value)
        {
            UpdateBoolProperty();
        }

        #endregion

        #region MonoBehaviour Functions

        private void Awake()
        {
            if (_boolPropertyBinders != null)
            {
                localBoolValue = _boolPropertyBinders[0].boolProperty;
                foreach (var boolPropertyBinder in _boolPropertyBinders)
                {
                    boolPropertyBinder.boolProperty = localBoolValue;
                }
            }
        }

        private void Update()
        {
            if (_boolPropertyBinders != null)
            {
                foreach (var propertyBinder in _boolPropertyBinders)
                {
                    if (propertyBinder.boolProperty != localBoolValue ||
                        (this.model != null && this.model.boolProperty != localBoolValue))
                    {
                        localBoolValue = propertyBinder.boolProperty;
                        if (this.model != null)
                        {
                            this.model.boolProperty = localBoolValue;
                        }
                    }
                }
            }
        }

        #endregion
    }
}