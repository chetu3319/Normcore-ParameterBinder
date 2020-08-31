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
    public class RealtimeColorSync : RealtimeComponent<RealtimeColorModel>
    {
        #region Property Binders

        [SerializeReference] [HideInInspector] ColorPropertyBinder[] _colorPropertyBinders = null;

        public ColorPropertyBinder[] PropertyBinders
        {
            get => (ColorPropertyBinder[]) _colorPropertyBinders.Clone();
            set => _colorPropertyBinders = value;
        }

        #endregion

        // Local Variable which will be synced with the network. 
        // Property binders will subscribe to this value to be in sync. 
        [HideInInspector] public Color localColorValue;

        #region Normcore Realtime Logic

        protected override void OnRealtimeModelReplaced(RealtimeColorModel previousModel,
            RealtimeColorModel currentModel)
        {
            if (previousModel != null)
            {
                previousModel.colorPropertyDidChange -= ModelOnColorPropertyDidChange;
            }

            if (currentModel != null)
            {
                if (!currentModel.isFreshModel)
                {
                    UpdateColorProperty();
                }

                currentModel.colorPropertyDidChange += ModelOnColorPropertyDidChange;
            }
        }

        private void UpdateColorProperty()
        {
            localColorValue = this.model.colorProperty;
            if (_colorPropertyBinders != null)
            {
                foreach (var propertyBinder in _colorPropertyBinders)
                {
                    propertyBinder.colorProperty = localColorValue;
                }
            }
        }

        private void ModelOnColorPropertyDidChange(RealtimeColorModel normcoreBoolModel, Color value)
        {
            UpdateColorProperty();
        }

        #endregion

        #region MonoBehaviour Functions

        private void Awake()
        {
            if (_colorPropertyBinders != null)
            {
                localColorValue = _colorPropertyBinders[0].colorProperty;
                foreach (var boolPropertyBinder in _colorPropertyBinders)
                {
                    boolPropertyBinder.colorProperty = localColorValue;
                }
            }
        }

        private void Update()
        {
            if (_colorPropertyBinders != null)
            {
                foreach (var propertyBinder in _colorPropertyBinders)
                {
                    if (propertyBinder.colorProperty != localColorValue ||
                        (this.model != null && this.model.colorProperty != localColorValue))
                    {
                        localColorValue = propertyBinder.colorProperty;
                        if (this.model != null)
                        {
                            this.model.colorProperty = localColorValue;
                        }
                    }
                }
            }
        }

        #endregion
    }
}