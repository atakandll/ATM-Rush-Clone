using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Input", menuName = ("ATM Rush/CD_Input"))]
    public class CD_Input : ScriptableObject
    {
        public InputData Data;
    }
}