using UnityEngine;
using UnityEngine.Assertions;

namespace EsDee
{
    public class GameMode : MonoBehaviour
    {
        public static GameMode Singleton
        {
            get; private set;
        }

        [SerializeField]
        GameBootModeSetting bootModeSetting;

        public GameBootMode BootMode => bootModeSetting.BootMode;

        void Awake()
        {
            Assert.IsNotNull(bootModeSetting);

            if (Singleton == null)
            {
                Singleton = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
