using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;

namespace EsDee
{
    public class NetworkCharacterMesh : NetworkBehaviour
    {
        [SerializeField]
        CharaPrefabList charaPrefabList;

        NetworkVariable<CharaCode> networkCharaCode = new();
        
        void Awake()
        {
            Assert.IsNotNull(charaPrefabList);
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                networkCharaCode.OnValueChanged += OnNetworkCharaCodeChanged;

                if (IsOwner)
                {
                    SetNetworkCharaCodeServerRpc(UserProfile.Singleton.CharacterCode);
                }
            }
        }

        [ServerRpc(RequireOwnership = true, Delivery = RpcDelivery.Reliable)]
        public void SetNetworkCharaCodeServerRpc(CharaCode charaCode, ServerRpcParams rpcParams = default)
        {
            if (charaCode == CharaCode.NotSelected)
            {
                NetworkManager.Singleton.DisconnectClient(rpcParams.Receive.SenderClientId);
                return;
            }

            networkCharaCode.Value = charaCode;
        }

        public void OnNetworkCharaCodeChanged(CharaCode prev, CharaCode next)
        {

        }
    }
}


