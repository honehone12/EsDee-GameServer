using UnityEngine;

namespace EsDee
{
    [CreateAssetMenu(fileName = "BigBulletSkillFunc", menuName = "Skill/BigBulletSkillFunc")]
    public class BigBulletSkillFunc : SkillFunc
    {
        [SerializeField]
        Bullet bullet;
        [SerializeField]
        float force = 1000.0f;
        [SerializeField]
        float torque = 100.0f;

        public override void ServerFire(in SkillParams skillParams)
        {
            var networkBullet = bullet.CreateOne(skillParams.origin);
            networkBullet.NetworkObjectComponent.Spawn(true);
            var rigidBody = networkBullet.RigidBodyComponent;
            rigidBody.AddTorque(new Vector3(
                torque * Random.Range(-10.0f, 10.0f),
                torque * Random.Range(-10.0f, 10.0f),
                torque * Random.Range(-10.0f, 10.0f)
            ), ForceMode.Impulse);
            rigidBody.AddForce(force * skillParams.direction, ForceMode.Impulse);
        }
    }
}
