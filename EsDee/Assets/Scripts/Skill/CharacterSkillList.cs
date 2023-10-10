using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EsDee
{
    [CreateAssetMenu(menuName = "Skill/CharacterSkillList", fileName = "CharacterSkillList")]
    public class CharacterSkillList : ScriptableObject
    {
        [SerializeField]
        List<CharacterSkill> characterSkillList = new();

        public CharacterSkill Find(CharacterSkillCode code, out bool ok)
        {
            Assert.IsTrue(code != CharacterSkillCode.NotSelected);
            var skill = characterSkillList.Find((cs) => cs.skillCode == code);
            Assert.IsNotNull(skill);
            ok = skill != null;
            return new CharacterSkill(skill);
        }
    }
}
