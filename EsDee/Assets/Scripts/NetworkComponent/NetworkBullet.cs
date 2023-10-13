using System.Collections;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

namespace EsDee
{
    [RequireComponent(typeof(NetworkObject))]
    [RequireComponent(typeof(NetworkRigidbody))]
    public class NetworkBullet : NetworkBehaviour
    {
        [SerializeField]
        float lifeTime = 3.0f;

        Rigidbody rigidBody;
        NetworkObject networkObject;

        public Rigidbody RigidBodyComponent => rigidBody;

        public NetworkObject NetworkObjectComponent => networkObject;

        void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            networkObject = GetComponent<NetworkObject>();
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                _ = StartCoroutine(LifeTimer());
            }
        }

        IEnumerator LifeTimer()
        {
            yield return new WaitForSeconds(lifeTime);
            networkObject.Despawn(true);
        }
    }
}
