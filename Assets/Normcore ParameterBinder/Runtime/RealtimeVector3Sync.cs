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
    public class RealtimeVector3Sync : RealtimeComponent<RealtimeVector3Model>
    {
        #region Parameter Binding

        [SerializeReference] [HideInInspector] private Vector3PropertyBinder[] _vector3PropertyBinders = null;

        public Vector3PropertyBinder[] Vector3PropertyBinders
        {
            get => (Vector3PropertyBinder[]) _vector3PropertyBinders.Clone();
            set => _vector3PropertyBinders = value;
        }

        #endregion

        // Local Variable which will be synced with the network. 
        // Property binders will subscribe to this value to be in sync. 
        [HideInInspector] public Vector3 localVector3Value;

        #region Normcore Realtime Logic

        protected override void OnRealtimeModelReplaced(RealtimeVector3Model previousModel,
            RealtimeVector3Model currentModel)
        {
            if (previousModel != null)
            {
                previousModel.vector3PropertyDidChange -= ModelOnvector3PropertyDidChange;
            }

            if (currentModel != null)
            {
                if (!currentModel.isFreshModel)
                {
                    UpdateVector3Property();
                }

                currentModel.vector3PropertyDidChange += ModelOnvector3PropertyDidChange;
            }
        }

        private void UpdateVector3Property()
        {
            localVector3Value = this.model.vector3Property;
            if (_vector3PropertyBinders != null)
            {
                foreach (var vector3PropertyBinder in _vector3PropertyBinders)
                {
                    vector3PropertyBinder.vector3Property = localVector3Value;
                }
            }
        }

        private void ModelOnvector3PropertyDidChange(RealtimeVector3Model realtimeVector3Model, Vector3 value)
        {
            UpdateVector3Property();
        }

        #endregion

        #region MonoBehaviour Functions

        private void Awake()
        {
            if (_vector3PropertyBinders != null)
            {
                localVector3Value = _vector3PropertyBinders[0].vector3Property;
                foreach (var vector3PropertyBinder in _vector3PropertyBinders)
                {
                    vector3PropertyBinder.vector3Property = localVector3Value;
                }
            }
        }

        private void Update()
        {
            if (_vector3PropertyBinders != null)
            {
                foreach (var vector3PropertyBinder in _vector3PropertyBinders)
                {
                    if (vector3PropertyBinder.vector3Property != localVector3Value ||
                        (this.model != null && this.model.vector3Property != localVector3Value))
                    {
                        localVector3Value = vector3PropertyBinder.vector3Property;
                        if (this.model != null)
                        {
                            this.model.vector3Property = localVector3Value;
                        }
                    }
                }
            }
        }

        #endregion
    }
}