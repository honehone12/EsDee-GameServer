using UnityEngine;

namespace EsDee
{
    public abstract class SkillFunc : ScriptableObject
    {
        public abstract void Fire(Vector3 origin);
    }
}
