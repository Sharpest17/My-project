using UnityEngine;
using System.Collections.Generic;

public abstract class CombatModifier : ScriptableObject
{
    public HookType [] hooks;

    public Combatant owner;

    public List<SkillTag> tags;

    public virtual void OnHook(HookType hook, CombatContext ctx) {}
}
