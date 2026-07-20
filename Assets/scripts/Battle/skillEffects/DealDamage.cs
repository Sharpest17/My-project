using UnityEngine;

[CreateAssetMenu(fileName = "DealDamageEffect", menuName = "RPG/SkillEffect/DealDamage")]
public class DealDamage : SkillEffect
{
    public override void Apply(Combatant user, Combatant target, Skill skill)
    {
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
        BattleManager.Instance.QueueDamage(ctx);
    }
}
