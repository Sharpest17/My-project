using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(
fileName = "Taunt",
menuName = "RPG/StatusEffect/Stealth")]
public class Stealth : StatusEffect
{
    public override void OnHook(
    HookType hook,
    CombatContext ctx
)
    {
        if(hook == HookType.ModifyTargets)
{
    TargetingContext targetCtx =
        ctx as TargetingContext;

    if(targetCtx == null || targetCtx.skill.targetType != TargetType.SingleEnemy)
        return;

    if(targetCtx.validTargets.Contains(owner))
    {
        targetCtx.validTargets.Remove(owner);
    }

    if(!targetCtx.hiddenTargets.Contains(owner))
    {
        targetCtx.hiddenTargets.Add(owner);
    }
}

    }
}
