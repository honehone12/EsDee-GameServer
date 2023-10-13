using UnityEngine;
using Unity.Netcode;

namespace EsDee
{
    public class SceneBoot : MonoBehaviour
    {
        void Start()
        {
            var bootMode = GameSetting.Singleton.BootSetting.BootMode;
            switch (bootMode)
            {
                case GameBootMode.Development:
                    NetworkManager.Singleton.StartHost();
                    if (Debug.isDebugBuild)
                    {
                        Debug.Log("Network booted as 'Host'");
                    }
                    break;
                case GameBootMode.ServerBuild:
                    NetworkManager.Singleton.StartServer();
                    if (Debug.isDebugBuild)
                    {
                        Debug.Log("Network booted as 'Server'");
                    }
                    break;
                case GameBootMode.ClientBuild:
                    NetworkManager.Singleton.StartClient();
                    if (Debug.isDebugBuild)
                    {
                        Debug.Log("Network booted as 'Client'");
                    }
                    break;
                default:
                    Debug.LogError("Unexpecte boot mode");
                    break;
            }
        }
    }
}


