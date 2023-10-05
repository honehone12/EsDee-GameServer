using UnityEngine;

namespace EsDee
{
    [CreateAssetMenu(fileName = "BigBulletSkillFunc", menuName = "Skill/BigBulletSkillFunc")]
    public class BigBulletSkill : SkillFunc
    {
        [SerializeField]
        Bullet bullet;

        public override void Fire(Vector3 origin)
        {
            var netBullet = bullet.GetOne(origin);
        }
    }
}


