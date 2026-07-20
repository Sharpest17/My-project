using UnityEngine;
using System.Collections.Generic;

public class ModifierManager
{
    List<Combatant> combatants;

    public ModifierManager(List<Combatant> combatants)
    {
        this.combatants = combatants;
    }

    public void Broadcast(HookType hook, CombatContext ctx)
    {
        foreach (var c in combatants)
        {
            foreach (var mod in c.GetAllModifiers())
        {
        mod.OnHook(hook, ctx);
            }
        }
    }
}
