using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EsDee
{
    [System.Serializable]
    public class CharacterSkill
    {
        public SkillCode skillCode;
        public SkillFunc skillFunc;
        public float intervalSec = 1.0f;

        public CharacterSkill(CharacterSkill other)
        {
            skillCode = other.skillCode;
            skillFunc = other.skillFunc;
            intervalSec = other.intervalSec;
        }
    }

    [CreateAssetMenu(menuName = "Skill/CharacterSkillList", fileName = "CharacterSkillList")]
    public class CharacterSkillList : ScriptableObject
    {
        [SerializeField]
        List<CharacterSkill> characterSkillList = new();

        public CharacterSkill Find(SkillCode code, out bool ok)
        {
            Assert.IsTrue(code != SkillCode.NotSelected);
            var skill = characterSkillList.Find((cs) => cs.skillCode == code);
            Assert.IsNotNull(skill);
            ok = skill != null;
            return new CharacterSkill(skill);
        }
    }
}
