using UnityEngine;
using UnityEngine.Assertions;

namespace EsDee
{
    [CreateAssetMenu(menuName = "Bullet", fileName = "Bullet")]
    public class Bullet : ScriptableObject
    {
        [SerializeField]
        GameObject bulletPrefab;

        public NetworkBullet GetOne(Vector3 origin)
        {
            var bullet = Instantiate(bulletPrefab, origin, Quaternion.identity)
                .GetComponent<NetworkBullet>();
            Assert.IsNotNull(bullet);
            Assert.IsNotNull(bullet.RigidBody);
            return bullet;
        }
    }
}


