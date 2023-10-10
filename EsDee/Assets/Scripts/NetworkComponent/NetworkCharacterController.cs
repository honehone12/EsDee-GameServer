using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using EsDee.Extensions;

namespace EsDee
{
    public class NetworkCharacterController : NetworkBehaviour
    {
        [Header("Physics")]
        [SerializeField]
        Rigidbody rigidBody;
        [SerializeField]
        float force = 5.0f;
        [SerializeField]
        float torque = 1.0f;
        [Header("Input")]
        [SerializeField]
        CharacterControlInput controlInput;
        [SerializeField]
        int inputBufferSize = 16;

        Transform rigidBodyTransform;
        InputBuffer inputBuffer;

        void Awake()
        {
            Assert.IsNotNull(controlInput);
            Assert.IsTrue(inputBufferSize > 0);
            Assert.IsNotNull(rigidBody);
            Assert.IsTrue(force > 0.0f);
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                inputBuffer = new(inputBufferSize);
            }

            rigidBodyTransform = rigidBody.transform;
        }

        void FixedUpdate()
        {
            if (Time.frameCount % 2 == 0)
            {
                return;
            }

            if (IsClient && IsOwner)
            {
                var controlBits = controlInput.GetCharacterControlInput();
                if (controlBits != default)
                {
                    InformCharacterControlInputServerRpc(controlBits);
                }
            }

            if (IsServer)
            {
                var nextBits = inputBuffer.DequeueOrDefault();
                if (nextBits != default)
                {
                    float depth = default;
                    float theta = default;
                    if (nextBits.IsBitOn(BitsUtil.ForwardBit))
                    {
                        depth += 1.0f;
                    }
                    if (nextBits.IsBitOn(BitsUtil.BackBit))
                    {
                        depth -= 1.0f;
                    }
                    if (nextBits.IsBitOn(BitsUtil.RightBit))
                    {
                        theta += 1.0f;
                    }
                    if (nextBits.IsBitOn(BitsUtil.LeftBit))
                    {
                        theta -= 1.0f;
                    }

                    rigidBody.AddForce(depth * force * rigidBodyTransform.forward, ForceMode.Impulse);
                    rigidBody.AddTorque(theta * torque * rigidBodyTransform.up, ForceMode.Impulse);
                }
            }
        }

        [ServerRpc(RequireOwnership = true, Delivery = RpcDelivery.Unreliable)]
        void InformCharacterControlInputServerRpc(byte bits, ServerRpcParams rpcParams = default)
        {
            inputBuffer.Enqueue(bits);
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer)
            {
                inputBuffer.Dispose();
            }
        }
    }
}
