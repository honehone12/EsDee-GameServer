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
                    GameManager.Singleton.OnServerBoot();
                    NetworkManager.Singleton.StartHost();
                    if (Debug.isDebugBuild)
                    {
                        Debug.Log("Network booted as 'Host'");
                    }
                    break;
                case GameBootMode.ServerBuild:
                    GameManager.Singleton.OnServerBoot();
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


