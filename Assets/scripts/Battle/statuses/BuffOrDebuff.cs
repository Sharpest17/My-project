using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "RPG/StatusEffect/BuffOrDebuff")]
public class BuffOrDebuff : StatusEffect
{
    public List<StatModifier> statModifiers;
    public List<CombatStatModifier>  combatStatModifiers;

    public override void OnHook(HookType hook, CombatContext ctx)
    {
        if(hook == HookType.ModifyStat)
        {
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
        if(hook == HookType.ModifyCombatStat)
        {
            CombatStatContext combatCtx = ctx as CombatStatContext;
            foreach(var mod in combatStatModifiers)
            {
                if(mod.combatStat == combatCtx.stat)
                {
                    combatCtx.value += mod.value;
                }
            }
        }
    }
}
