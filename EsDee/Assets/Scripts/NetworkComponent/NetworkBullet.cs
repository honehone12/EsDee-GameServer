using UnityEngine;
using Unity.Netcode;

namespace EsDee
{
    [RequireComponent(typeof(Rigidbody))]
    public class NetworkBullet : NetworkBehaviour
    {
        Rigidbody rigidBody;

        public Rigidbody RigidBody
        {
            get
            {
                if (rigidBody == null)
                {
                    rigidBody = GetComponent<Rigidbody>();
                }
                return rigidBody;
            }
        }
    }
}
