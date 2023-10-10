using UnityEngine;
using UnityEngine.Assertions;

namespace EsDee
{
    public class CharaSelectUI : MonoBehaviour
    {
        static CharaSelectUI Instance;

        public static bool IsCharaSelectScene => Instance != null;

        [SerializeField]
        SceneLoader sceneLoader;

        void Awake()
        {
            Assert.IsNotNull(sceneLoader);

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void OnDestroy()
        {
            Assert.IsTrue(Instance == this);
            Instance = null;
        }

        public void OnKohakuButton()
        {
            OnCharaSelect(CharaCode.Kohaku);
        }

        public void OnMisakiButton()
        {
            OnCharaSelect(CharaCode.Misaki);
        }

        public void OnYukoButton()
        {
            OnCharaSelect(CharaCode.Yuko);
        }

        public void OnCharaSelect(CharaCode charaCode)
        {
            UserProfile.Singleton.CharacterCode = charaCode;
            sceneLoader.Load();
        }
    }
}
