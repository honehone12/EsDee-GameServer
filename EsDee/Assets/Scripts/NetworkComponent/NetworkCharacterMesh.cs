using System.Collections;
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
        GameObject meshInstance;
        
        void Awake()
        {
            Assert.IsNotNull(charaPrefabList);
        }

        void OnEnable()
        {
            networkCharaCode.OnValueChanged += OnNetworkCharaCodeChanged;
        }

        void OnDisable()
        {
            networkCharaCode.OnValueChanged -= OnNetworkCharaCodeChanged;
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                if (IsOwner)
                {
                    SetNetworkCharaCodeServerRpc(UserProfile.Singleton.CharacterCode);
                }
                else
                {
                    _ = StartCoroutine(WaitForVariableInitialized());
                }
            }
        }

        IEnumerator WaitForVariableInitialized()
        {
            yield return new WaitWhile(() => networkCharaCode.Value == CharaCode.NotSelected);

            ChangeCharaMesh(networkCharaCode.Value);
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
            Assert.IsFalse(next == CharaCode.NotSelected);

            if (IsClient)
            {
                ChangeCharaMesh(next);
            }
        }

        void ChangeCharaMesh(CharaCode code)
        {
            var charaPrefab = charaPrefabList.Find(code, out var ok);
            if (ok)
            {
                if (meshInstance != null)
                {
                    Destroy(meshInstance);
                }
                meshInstance = Instantiate(charaPrefab.prefab, transform);
            }
        }
    }
}
