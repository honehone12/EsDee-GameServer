using UnityEngine;

namespace EsDee
{
    [CreateAssetMenu(fileName = "BigBulletCharaSkillFunc", menuName = "Skill/BigBulletCharaSkillFunc")]
    public class BigBulletCharacterSkill : SkillFunc
    {
        [SerializeField]
        Bullet bullet;

        public override void Fire(Vector3 origin)
        {
            var netBullet = bullet.GetOne(origin);
        }
    }
}


