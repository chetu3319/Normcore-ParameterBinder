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
    public class RealtimeStringSync : RealtimeComponent<RealtimeStringModel>
    {
        #region Property Binders

        [SerializeReference] [HideInInspector] StringPropertyBinder[] _stringPropertyBinders = null;

        public StringPropertyBinder[] PropertyBinders
        {
            get => (StringPropertyBinder[]) _stringPropertyBinders.Clone();
            set => _stringPropertyBinders = value;
        }

        #endregion

        // Local Variable which will be synced with the network. 
        // Property binders will subscribe to this value to be in sync. 
        [HideInInspector] public string localStringValue;

        #region Normcore Realtime Logic

        protected override void OnRealtimeModelReplaced(RealtimeStringModel previousModel,
            RealtimeStringModel currentModel)
        {
            if (previousModel != null)
            {
                previousModel.stringPropertyDidChange -= ModelOnStringPropertyDidChange;
            }

            if (currentModel != null)
            {
                if (!currentModel.isFreshModel)
                {
                    UpdateStringProperty();
                }

                currentModel.stringPropertyDidChange += ModelOnStringPropertyDidChange;
            }
        }

        private void UpdateStringProperty()
        {
            localStringValue = this.model.stringProperty;
            if (_stringPropertyBinders != null)
            {
                foreach (var propertyBinder in _stringPropertyBinders)
                {
                    propertyBinder.stringProperty = localStringValue;
                }
            }
        }

        private void ModelOnStringPropertyDidChange(RealtimeStringModel normcoreBoolModel, string value)
        {
            UpdateStringProperty();
        }

        #endregion

        #region MonoBehaviour Functions

        private void Awake()
        {
            if (_stringPropertyBinders != null)
            {
                localStringValue = _stringPropertyBinders[0].stringProperty;
                foreach (var boolPropertyBinder in _stringPropertyBinders)
                {
                    boolPropertyBinder.stringProperty = localStringValue;
                }
            }
        }

        private void Update()
        {
            if (_stringPropertyBinders != null)
            {
                foreach (var propertyBinder in _stringPropertyBinders)
                {
                    if (propertyBinder.stringProperty != localStringValue ||
                        (this.model != null && this.model.stringProperty != localStringValue))
                    {
                        localStringValue = propertyBinder.stringProperty;
                        if (this.model != null)
                        {
                            this.model.stringProperty = localStringValue;
                        }
                    }
                }
            }
        }

        #endregion
    }
}