using System;

namespace Runtime.Keys
{
    [Serializable]
    public struct SaveGameDataParams
    {
        public int Level;
        public float Money;
        public int IncomeLevel;
        public int StackLevel;
    }
}