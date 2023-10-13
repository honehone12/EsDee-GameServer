using UnityEngine;

public readonly struct SkillParams
{
    public readonly Vector3 origin;
    public readonly Vector3 direction;

    public SkillParams(Vector3 origin, Vector3 direction)
    {
        this.origin = origin;
        this.direction = direction;
    }
}
