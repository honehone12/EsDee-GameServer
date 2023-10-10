using UnityEngine;

namespace EsDee
{
    [CreateAssetMenu(menuName = "Input/CharacterSkill", fileName = "CharacterSkillInput")]
    public class CharacterSkillInput : ScriptableObject
    {
        [SerializeField]
        DigitalInputFunc characterSkillMouseR;
        [SerializeField]
        DigitalInputFunc characterSkillMouseL;

        public byte GetCharacterSkillInput()
        {
            byte bits = 0b0000_0000;
            if (characterSkillMouseR.IsOn)
            {
                bits |= BitsUtil.MouseRBit;
            }
            if (characterSkillMouseL.IsOn)
            {
                bits |= BitsUtil.MouseLBit;
            }

            return bits;
        }
    }
}
