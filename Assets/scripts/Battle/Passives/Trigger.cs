using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Passive", menuName = "RPG/PassiveEffect/Trigger")]
public class Trigger : PassiveSkill
{
    public Skill triggerable;
    public override void OnHook(HookType hook, CombatContext ctx)
    {
        if(hook == HookType.OnMiss && ctx is DamageContext dmg)
        {
            if(dmg.target == owner){
            BattleManager.Instance.AddMessage($"{owner.character.characterName}'s passive triggered! it's jump time!");

            BattleManager.Instance.TriggerSkill(owner, triggerable, ctx.attacker);
            }
        }
    }
}
