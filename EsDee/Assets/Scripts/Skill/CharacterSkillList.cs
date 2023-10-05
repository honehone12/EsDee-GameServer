using System.Collections.Generic;
using UnityEngine;

namespace EsDee
{
    [CreateAssetMenu(menuName = "Skill/CharacterSkillList", fileName = "CharacterSkillList")]
    public class CharacterSkillList : ScriptableObject
    {
        [SerializeField]
        List<CharacterSkill> characterSkillList = new();
    }
}
