using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using Unity.Collections;
using TMPro;

namespace EsDee
{
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField]
        TMP_Text nameTag;

        NetworkVariable<FixedString32Bytes> networkName = new();

        void Awake()
        {
            Assert.IsNotNull(nameTag);
        }

        void OnEnable()
        {
            networkName.OnValueChanged += OnNetworkNameChanged;
        }

        void OnDisable()
        {
            networkName.OnValueChanged -= OnNetworkNameChanged;
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                if (IsOwner)
                {
                    SetNetworkNameServerRpc(UserProfile.Singleton.Name32Bytes);
                }
                else
                {
                    _ = StartCoroutine(WaitForVariableInitialized());
                }
            }
        }

        IEnumerator WaitForVariableInitialized()
        {
            yield return new WaitWhile(() => networkName.Value.IsEmpty);

            UpdataNameTag(networkName.Value.ToString());
        }

        [ServerRpc(RequireOwnership = true, Delivery = RpcDelivery.Reliable)]
        public void SetNetworkNameServerRpc(FixedString32Bytes name, ServerRpcParams rpcParams = default)
        {
            if (name.IsEmpty)
            {
                NetworkManager.Singleton.DisconnectClient(rpcParams.Receive.SenderClientId);
                return;
            }

            networkName.Value = name;
        }

        public void OnNetworkNameChanged(FixedString32Bytes prev, FixedString32Bytes next)
        {
            Assert.IsFalse(next.IsEmpty);

            if (IsClient)
            {
                UpdataNameTag(next.ToString());
            }
        }

        void UpdataNameTag(string name)
        {
            if (!string.IsNullOrEmpty(name) && name.Length <= StringUtil.UserNameLengthLimitUtf8)
            {
                nameTag.text = name;
            }
        }
    }
}
