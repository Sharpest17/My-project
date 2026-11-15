using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "lifestealOrRecoil", menuName = "RPG/SkillEffect/lifestealOrRecoil")]
public class lifestealOrRecoil : SkillEffect
{
    [Tooltip("percentage based on last attack's final damage")]
    public float lifestealPercentage;
    public float RecoilPercentage;
    [Tooltip("true for lifesteal, negative for recoil")]
    public bool isLifesteal;
    public bool isRecoil;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Apply(Combatant user, List<Combatant> targets, Skill skill, SkillContext skillctx)
    {
        if(isLifesteal)
        {
            HealContext ctx = new HealContext(user, user, skill.power, skill.attackingStat, skill);
            float lifesteal = skillctx.lastAttackDamage*lifestealPercentage;
            ctx.finalHeal = Mathf.FloorToInt(lifesteal);
            user.RestoreHP(ctx);
        }
        if(isRecoil)
        {
            DamageContext ctx = new DamageContext(
            user,
            user,
            skill.power, //will be calculated by the calculate damage call
            DamageSource.Skill,
            skill.damageType,
            skill.attackingStat,
            skill.defendStat,
            skill
            );
            int recoil = Mathf.FloorToInt(skillctx.lastAttackDamage*RecoilPercentage);
            ctx.finalDamage = recoil;
            BattleManager.Instance.AddMessage($"{user.character.characterName} suffers {recoil} recoil");
            user.TakeDamage(ctx, skillctx);
        }
    }
}
