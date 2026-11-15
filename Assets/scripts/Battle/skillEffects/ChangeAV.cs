using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ChangeAV", menuName = "RPG/SkillEffect/ChangeAV")]
public class ChangeAV : SkillEffect
{
    public float ActionValuePercentage;
    public override void Apply(Combatant user, List<Combatant> targets, Skill skill, SkillContext skillctx)
    {
        foreach(Combatant target in targets)
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
}
