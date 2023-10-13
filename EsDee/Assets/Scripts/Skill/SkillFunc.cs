using UnityEngine;

namespace EsDee
{
    public abstract class SkillFunc : ScriptableObject
    {
        public abstract void ServerFire(in SkillParams skillParams);

        public virtual void ClientFire(in SkillParams skillParams) { }
    }
}
