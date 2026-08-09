using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "RPG/StatusEffect/BuffOrDebuff")]
public class BuffOrDebuff : StatusEffect
{
    public List<StatModifier> statModifiers;

    public override void OnHook(HookType hook, CombatContext ctx)
    {
        if(hook != HookType.ModifyStat)
            return;

        StatContext statCtx = ctx as StatContext;

        foreach(var mod in statModifiers)
        {
            if(mod.stat == statCtx.stat)
            {
                statCtx.multiplier += mod.multiplier;
                statCtx.baseBonus += mod.baseBonus;
            }
        }
    }
}
