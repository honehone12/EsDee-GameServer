using UnityEngine;
using UnityEngine.Assertions;
using Cinemachine;

namespace EsDee
{
    public class SceneCamera : MonoBehaviour
    {
        public static SceneCamera Main { get; private set; }

        [SerializeField]
        Camera renderCamera;
        [SerializeField]
        CinemachineVirtualCamera virtualCamera;

        public Camera RenderCamera => renderCamera;

        void Awake()
        {
            Assert.IsNotNull(renderCamera);
            Assert.IsNotNull(virtualCamera);

            if (Main == null)
            {
                Main = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetFollowTarget(Transform target)
        {
            virtualCamera.Follow = target;
        }

        public void SetLookAtTarget(Transform target)
        {
            virtualCamera.LookAt = target;
        }
    }
}
