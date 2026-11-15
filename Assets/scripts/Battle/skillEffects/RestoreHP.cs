using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "RestoreHPEffect", menuName = "RPG/SkillEffect/RestoreHP")]
public class RestoreHP : SkillEffect
{
    bool lastHealCrit;
    public override void Apply(Combatant user, List<Combatant> targets, Skill skill, SkillContext skillctx)
    {
        foreach(Combatant target in targets)
        {
        HealContext ctx = new HealContext(user, target, skill.power, skill.attackingStat, skill);

        BattleMath.CalculateHealing(ctx);

        target.RestoreHP(ctx, skillctx);
        
                lastHealCrit |= ctx.critical;
        }
        skillctx.lastHealCrit = lastHealCrit;
    }
}
