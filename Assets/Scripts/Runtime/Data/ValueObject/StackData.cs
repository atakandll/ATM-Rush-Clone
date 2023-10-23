using System;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct StackData
    {
        public float CollectableOffsetInStack;
        [Range(0.1f, 0.8f)] public float LerpSpeed;
        [Range(0, 0.2f)] public float StackAnimDuraction;
        [Range(1f, 3f)] public float StackScaleValue;
        [Range(1f, 10f)] public float JumpForce;
        public float JumpItemsClampX;
    }
}