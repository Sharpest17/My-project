using UnityEngine;

[CreateAssetMenu(fileName = "ChangeAV", menuName = "RPG/SkillEffect/ChangeAV")]
public class ChangeAV : SkillEffect
{
    public float ActionValuePercentage;
    public override void Apply(Combatant user, Combatant target, Skill skill, SkillContext skillctx)
    {
        target.actionValue -= BattleManager.Instance.BaseAVCost*ActionValuePercentage;
    }
}
