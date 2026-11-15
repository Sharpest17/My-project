using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "RestoreHPEffect", menuName = "RPG/SkillEffect/RestoreHP")]
public class RestoreHP : SkillEffect
{
    public override void Apply(Combatant user, List<Combatant> targets, Skill skill, SkillContext skillctx)
    {
        bool lastHealCrit = false;
        int lastHealAmount = 0;
        foreach(Combatant target in targets)
        {
        HealContext ctx = new HealContext(user, target, skill.power, skill.attackingStat, skill);

        BattleMath.CalculateHealing(ctx);

        target.RestoreHP(ctx, skillctx);

            lastHealCrit |= ctx.critical;
            lastHealAmount += skillctx.lastHealAmount;
        }
        skillctx.lastHealAmount = lastHealAmount;
        skillctx.lastHealCrit = lastHealCrit;
        if(targets.Count >1)
        {
            BattleManager.Instance.AddMessage($"{user.character.characterName} restored hp to multiple targets");
        }
        else
        {
            BattleManager.Instance.AddMessage($"{user.character.characterName} restored {lastHealAmount} hp to {targets[0].character.characterName}");
        }
    }
}
