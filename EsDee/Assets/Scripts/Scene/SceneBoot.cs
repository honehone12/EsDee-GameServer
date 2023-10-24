using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

namespace EsDee
{
    public class SceneBoot : MonoBehaviour
    {
        void Start()
        {
            var gm = GameManager.Singleton;
            var nm = NetworkManager.Singleton;
            var setting = GameSetting.Singleton;
            var bootMode = setting.BootSetting.BootMode;
            if (nm.TryGetComponent<UnityTransport>(out var transport))
            {
                switch (bootMode)
                {
                    case GameBootMode.Development:
                        gm.OnServerBoot();
                        transport.SetConnectionData("127.0.0.1", 9999);
                        nm.StartHost();
                        if (Debug.isDebugBuild)
                        {
                            Debug.Log("Network booted as 'Host'");
                        }
                        break;
                    case GameBootMode.ServerBuild:
                        gm.OnServerBoot();
                        // set listen address here when server setting is implemented
                        transport.SetConnectionData("0.0.0.0", 9999);
                        nm.StartServer();
                        if (Debug.isDebugBuild)
                        {
                            Debug.Log("Network booted as 'Server'");
                        }
                        break;
                    case GameBootMode.ClientBuild:
                        if (setting.IsGameServerIpSet)
                        {
                            var address = setting.GetGameServerUrl(out var port);
                            transport.SetConnectionData(address, port);
                            nm.StartClient();
                            if (Debug.isDebugBuild)
                            {
                                Debug.Log("Network booted as 'Client'");
                            }
                        }
                        break;
                    default:
                        Debug.LogError("Unexpecte boot mode");
                        break;
                }
            }

            
        }
    }
}


