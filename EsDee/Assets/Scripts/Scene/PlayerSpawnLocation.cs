using UnityEngine;

namespace EsDee
{
    public class PlayerSpawnLocation : MonoBehaviour
    {
        [SerializeField]
        Color gizmoColor = Color.red;
        [SerializeField]
        float playerRadius = 0.3f;

        void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            var t = transform;
            var pos = transform.position;
            Gizmos.DrawSphere(pos, playerRadius);
            Gizmos.DrawRay(pos, t.forward);
        }
    }
}
