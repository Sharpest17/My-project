using UnityEngine;

[CreateAssetMenu(fileName = "ChangeAV", menuName = "RPG/SkillEffect/ChangeAV")]
public class ChangeAV : SkillEffect
{
    public float ActionValuePercentage;
    public override void Apply(Combatant user, Combatant target, Skill skill, SkillContext skillctx)
    {
        target.actionValue -= BattleManager.Instance.BaseAVCost*ActionValuePercentage;
        if(ActionValuePercentage>0)
        {
            BattleManager.Instance.AddMessage($"{user.character.characterName} advanced {target.character.characterName} by {ActionValuePercentage*100}%");
        }
        if(ActionValuePercentage<0)
        {
            BattleManager.Instance.AddMessage($"{user.character.characterName} delayed {target.character.characterName} by {ActionValuePercentage*(-100)}%");
        }
        
    }
}
