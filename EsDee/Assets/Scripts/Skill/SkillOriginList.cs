using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EsDee
{
    [System.Serializable]
    public class SkillOrigin
    {
        public Transform origin;
        public SkillCode skillCode;
    }

    [System.Serializable]
    public class SkillOriginList
    {
        [SerializeField]
        List<SkillOrigin> skillOriginList = new();

        public SkillOrigin Find(SkillCode code, out bool ok)
        {
            Assert.IsFalse(code == SkillCode.NotSelected);
            var origin = skillOriginList.Find((so) => so.skillCode == code);
            Assert.IsNotNull(origin);
            ok = origin != null;
            return origin;
        }
    }
}
