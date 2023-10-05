using UnityEngine;

namespace EsDee
{
    public abstract class DigitalInputFunc : ScriptableObject
    {
        public abstract bool IsOn { get; }
    }
}
