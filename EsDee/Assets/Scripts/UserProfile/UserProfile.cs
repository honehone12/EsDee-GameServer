using UnityEngine;
using UnityEngine.Assertions;
using Unity.Collections;

namespace EsDee
{
    public class UserProfile : MonoBehaviour
    {
        public static UserProfile Singleton
        {
            get; private set;
        }

        string nameCache;
        CharaCode charaCodeChace;

        public CharaCode CharacterCode => charaCodeChace;
        public FixedString32Bytes Name => new FixedString32Bytes(nameCache);

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

        public void SetName(string name)
        {
            Assert.IsFalse(string.IsNullOrEmpty(name));
            if (InputUserNameUI.IsInputUserNameScene)
            {
                nameCache = name;
            }
        }

        public void SetCharaCode(CharaCode charaCode)
        {
            Assert.IsFalse(charaCode == CharaCode.NotSelected);
            if (CharaSelectUI.IsCharaSelectScene)
            {
                charaCodeChace = charaCode;
            }
        }
    }
}
