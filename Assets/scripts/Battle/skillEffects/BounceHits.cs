using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BounceHits", menuName = "RPG/SkillEffect/BounceHits")]
public class BounceHits : SkillEffect
{
    public int bounceCount;
    
    public override void Apply(Combatant user, Combatant target, Skill skill, SkillContext skillctx)
    {
        for(int i = 0; i < bounceCount; i++)
        {
            List<Combatant> targets =
        BattleManager.Instance.GetAllAliveEnemies(user);

        if(targets.Count == 0)
    {
        break;
    }

        Combatant newTarget =
        targets[Random.Range(0, targets.Count)];

        DamageContext ctx = new DamageContext(
        user,
        newTarget,
        skill.power, //will be calculated by the calculate damage call
        DamageSource.Skill,
        skill.damageType,
        skill.attackingStat,
        skill.defendStat,
        skill
        );
        BattleMath.CalculateDamage(ctx);
        newTarget.TakeDamage(ctx, skillctx);
        }    
    }
}
