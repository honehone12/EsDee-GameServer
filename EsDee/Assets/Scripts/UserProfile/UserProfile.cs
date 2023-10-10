using UnityEngine;
using UnityEngine.Assertions;
using Unity.Collections;

namespace EsDee
{
    public class UserProfile : MonoBehaviour
    {
        public static UserProfile Singleton { get; private set; }

        string userName;
        CharaCode charaCode;
        SkillSlot skillSlot;

        public CharaCode CharacterCode
        {
            get => charaCode;
            set 
            {
                Assert.IsFalse(value == CharaCode.NotSelected);
                if (CharaSelectUI.IsCharaSelectScene)
                {
                    charaCode = value;
                }
            }
        }

        public string Name
        {
            set
            {
                Assert.IsFalse(string.IsNullOrEmpty(value));
                if (InputUserNameUI.IsInputUserNameScene)
                {
                    userName = value;
                }
            }
        }

        public FixedString32Bytes Name32Bytes => new FixedString32Bytes(userName);

        public CharacterSkillCode SkillMouseR
        {
            set
            {
                Assert.IsFalse(value == CharacterSkillCode.NotSelected);
                if (SkillSelectUI.IsSkillSelectScene)
                {
                    skillSlot.charaSkillMouseR = value;
                }
            }
        }

        public CharacterSkillCode SkillMouseL
        {
            set
            {
                Assert.IsFalse(value == CharacterSkillCode.NotSelected);
                if (SkillSelectUI.IsSkillSelectScene)
                {
                    skillSlot.charaSkillMouseL = value;
                }
            }
        }

        public SkillSlot SkillSlot => skillSlot;

        void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
