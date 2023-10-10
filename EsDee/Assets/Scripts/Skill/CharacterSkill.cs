namespace EsDee
{
    [System.Serializable]
    public class CharacterSkill
    {
        public CharacterSkillCode skillCode;
        public SkillFunc skillFunc;
        public float intervalSec = 1.0f;

        public CharacterSkill(CharacterSkill other)
        {
            skillCode = other.skillCode;
            skillFunc = other.skillFunc;
            intervalSec = other.intervalSec;
        }
    }
}
