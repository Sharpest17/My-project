using UnityEngine;
using System.Collections.Generic;

public abstract class SkillEffect : ScriptableObject
{
    public abstract void Apply(Combatant user, List<Combatant> targets, Skill skill, SkillContext skillCtx);
}
