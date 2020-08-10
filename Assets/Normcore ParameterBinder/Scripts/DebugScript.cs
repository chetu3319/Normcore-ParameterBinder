using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{ 
   [SerializeField]
   private bool booleanValue;

   public bool BooleanValue
   {
      get => booleanValue;
      set => booleanValue = value;
   }

   [SerializeField][Range(0, 10)] private float floatValue;

   public float FloatValue
   {
      get => floatValue;
      set => floatValue = value;
   }
}
