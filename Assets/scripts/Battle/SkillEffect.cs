using UnityEngine;

public abstract class SkillEffect : ScriptableObject
{
    public abstract void Apply(Combatant user, Combatant target, Skill skill);
}
