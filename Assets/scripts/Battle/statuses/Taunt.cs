using UnityEngine;
using System.Collections.Generic;
using System.Linq;


[CreateAssetMenu(
fileName = "Taunt",
menuName = "RPG/StatusEffect/Taunt")]
public class Taunt : StatusEffect
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


    List<Combatant> taunts =
        targetCtx.validTargets
        .Where(t =>
            t.statusEffects.Any(
                s => s is Taunt
            ))
        .ToList();

    if(taunts.Count > 0)
    {
        targetCtx.validTargets =
            taunts;
    }
}
    }
}