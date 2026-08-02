using UnityEngine;

[CreateAssetMenu(fileName = "RestoreHPEffect", menuName = "RPG/SkillEffect/RestoreHP")]
public class RestoreHP : SkillEffect
{
    public override void Apply(Combatant user, Combatant target, Skill skill, SkillContext skillctx)
    {
        HealContext ctx = new HealContext(user, target, skill.power, skill.attackingStat, skill);

        BattleMath.CalculateHealing(ctx);

        BattleManager.Instance.QueueHealing(ctx);
    }
}
