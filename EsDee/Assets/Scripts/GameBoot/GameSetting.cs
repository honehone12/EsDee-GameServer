using UnityEngine;
using UnityEngine.Assertions;

namespace EsDee
{
    public class GameSetting : MonoBehaviour
    {
        public static GameSetting Singleton
        {
            get; private set;
        }

        [SerializeField]
        GameBootSetting bootSetting;

        public GameBootSetting BootSetting => bootSetting;


        void Awake()
        {
            Assert.IsNotNull(bootSetting);

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
