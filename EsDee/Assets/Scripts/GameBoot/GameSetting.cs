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
        
        string gameServerAddress = null;
        ushort gameServerPort = 0;

        public bool IsGameServerIpSet =>
            !string.IsNullOrEmpty(gameServerAddress) && gameServerPort != 0;

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

        public string GetGameServerUrl(out ushort port)
        {
            port = gameServerPort;
            return gameServerAddress;
        }

        public bool TrySetGameServerUrl(string address, string port)
        {
            if (ushort.TryParse(port, out gameServerPort))
            {
                gameServerAddress = address;
                return true;
            }

            return false;
        }
    }
}
