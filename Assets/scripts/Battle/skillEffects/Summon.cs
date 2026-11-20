using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "StatusOnCrit", menuName = "RPG/SkillEffect/Summon")]
public class Summon : SkillEffect
{
    public Character summon;

    public int SummonLimit = 1;
    public bool removedOnDefeat;
    public float startingAV = 1f;
    public override void Apply(Combatant user, List<Combatant> targets, Skill skill, SkillContext skillctx)
    {
        int summonCount = 0;
        foreach(var combatant in BattleManager.Instance.combatants)
        {
            if(combatant.character == summon)
            {
                summonCount++;
            }
        }
        if(summonCount >= SummonLimit)
        {
            BattleManager.Instance.AddMessage($"the max summon limit of {summon.characterName} has already been reached");
        }
        else{
        BattleManager.Instance.AddMessage($"{user.character.characterName} has summoned a {summon.characterName}");
        Combatant summoned = BattleManager.Instance.UnitEnters(summon, startingAV, removedOnDefeat);
        BattleManager.Instance.battleUI.SetupCombatHUD(BattleManager.Instance.combatants);
        }
    }
}
