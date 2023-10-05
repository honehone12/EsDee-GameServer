using UnityEngine;

namespace EsDee
{
    [CreateAssetMenu(menuName = "Input/CharacterControl", fileName = "CharacterControlInput")]
    public class CharacterControlInput : ScriptableObject
    {
        [SerializeField]
        DigitalInputFunc forwardInputFunc;
        [SerializeField]
        DigitalInputFunc backInputFunc;
        [SerializeField]
        DigitalInputFunc rightInputFunc;
        [SerializeField]
        DigitalInputFunc leftInputFunc;

        public byte GetCharacterControlInput()
        {
            byte bits = 0b0000_0000;
            if (forwardInputFunc.IsOn)
            {
                bits |= BitsUtil.ForwardBit;
            }
            if (backInputFunc.IsOn)
            {
                bits |= BitsUtil.BackBit;
            }
            if (rightInputFunc.IsOn)
            {
                bits |= BitsUtil.RightBit;
            }
            if (leftInputFunc.IsOn)
            {
                bits |= BitsUtil.LeftBit;
            }

            return bits;
        }
    }
}
