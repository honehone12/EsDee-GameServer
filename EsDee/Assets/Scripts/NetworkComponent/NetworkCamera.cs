using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;

namespace EsDee
{
    public class NetworkCamera : NetworkBehaviour
    {
        [SerializeField]
        Transform cameraTarget;
        [Header("UI")]
        [SerializeField]
        Transform uiTransform;
        [SerializeField]
        Canvas playerCanvas;

        void Awake()
        {
            Assert.IsNotNull(cameraTarget);
            Assert.IsNotNull(uiTransform);
            Assert.IsNotNull(playerCanvas);
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient && IsOwner)
            {
                var sceneCamera = SceneCamera.Main;
                sceneCamera.SetFollowTarget(cameraTarget);
                sceneCamera.SetLookAtTarget(cameraTarget);
                playerCanvas.worldCamera = sceneCamera.RenderCamera;
            }
        }

        void LateUpdate()
        {
            if (IsClient && !IsOwner)
            {

            }
        }
    }
}
