using Unity.Netcode;
using Unity.Collections;

namespace EsDee
{
    public class NetworkPlayer : NetworkBehaviour
    {
        NetworkVariable<FixedString32Bytes> networkName = new();

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                networkName.OnValueChanged += OnNetworkNameChanged;

                if (IsOwner)
                {
                    SetNetworkNameServerRpc(UserProfile.Singleton.Name);
                }
            }
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

        }
    }
}


