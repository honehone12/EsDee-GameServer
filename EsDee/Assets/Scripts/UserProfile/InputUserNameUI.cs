using UnityEngine;
using UnityEngine.Assertions;

namespace EsDee
{
    public class InputUserNameUI : MonoBehaviour
    {
        static InputUserNameUI Instance;
        
        public static bool IsInputUserNameScene => Instance != null;

        [SerializeField]
        SceneLoader sceneLoader;

        string nameBuffer = StringUtil.DefaultUserName;

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

        public void OnInputName(string name)
        {
            if (!string.IsNullOrEmpty(name) &&
                name.Length > 0 &&
                name.Length <= StringUtil.UserNameLengthLimitUtf8)
            {
                nameBuffer = name;
            }
        }

        public void OnOKButton()
        {
            UserProfile.Singleton.Name = nameBuffer;
            sceneLoader.Load();
        }
    }
}
