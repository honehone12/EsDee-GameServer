using UnityEngine;
using Unity.Netcode;

namespace EsDee
{
    public class FallingTrigger : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            var gm = GameManager.Singleton;
            if (gm.IsServer)
            {
                if (other.TryGetComponent<NetworkPlayer>(out var np))
                {
                    gm.EnqueueFalledPlayer(np.OwnerClientId);
                }
            }
        }
    }
}
