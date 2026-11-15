using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "DealDamageEffect", menuName = "RPG/SkillEffect/DealDamage")]
public class DealDamage : SkillEffect
{
    public override void Apply(Combatant user, List<Combatant> targets, Skill skill, SkillContext skillctx)
    {
        bool lastHit = false;
        bool lastCrit = false;
        bool lastKOd = false;

        int lastAttackDamage = 0;

        
        foreach(Combatant target in targets){
        DamageContext ctx = new DamageContext(
        user,
        target,
        skill.power, //will be calculated by the calculate damage call
        DamageSource.Skill,
        skill.damageType,
        skill.attackingStat,
        skill.defendStat,
        skill
        );
        BattleMath.CalculateDamage(ctx);
        target.TakeDamage(ctx, skillctx);
        lastHit |= skillctx.lastHit;
        lastCrit |= skillctx.lastCrit;
        lastKOd |= skillctx.lastKOd;
        lastAttackDamage += ctx.finalDamage;
    }
        skillctx.lastHit = lastHit;
        skillctx.lastCrit = lastCrit;
        skillctx.lastKOd = lastKOd;
        skillctx.lastAttackDamage = lastAttackDamage;
    }
}
