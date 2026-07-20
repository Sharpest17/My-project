using UnityEngine;
using System.Collections.Generic;

public class EnemyAI
{
    public AIProfile profile;
    private BattleManager battleManager;


    public EnemyAI(BattleManager bm, AIProfile profile)
    {
        this.battleManager = bm;
        this.profile = profile;
    }
    public Skill ChooseSkill(Combatant user, BattleManager battle)
{
    var skills = user.GetAvailableSkills();

    if (skills.Count == 0)
        return null;

    Dictionary<Skill, int> weights = new Dictionary<Skill, int>();

    foreach (var skill in skills)
    {
        int weight = 1;

        if ((skill.intents & IntentTag.Offensive) != 0)
            weight += 10;

        if (user.HPPercent < 0.4f &&
            (skill.intents & IntentTag.Defensive) != 0)
        {
            weight += 30;
        }

        weights[skill] = weight;

        Debug.Log($"{skill.name} weight: {weight}");
    }

    int totalWeight = 0;

    foreach (var pair in weights)
    {
        totalWeight += pair.Value;
    }

    int roll = Random.Range(0, totalWeight);
    Debug.Log($"{roll}");

    int current = 0;

    foreach (var pair in weights)
    {
        current += pair.Value;

        if (roll < current)
        {
            Debug.Log($"AI chose {pair.Key.name}");
            return pair.Key;
        }
    }

    return skills[0];
}

    public List<Combatant> ChooseTargets(Combatant user, Skill skill, List<Combatant> candidates)
    {
        if (candidates.Count == 0)
        return candidates;
    
    Debug.Log($"skill target type is {skill.targetType}");

    // AoE → use all
    if (skill.targetType == TargetType.AllEnemies ||
        skill.targetType == TargetType.AllAllies)
    {
        Debug.Log($"aoe detected, selecting all targets");
        return candidates;
    }

    Dictionary<Combatant, int> weights = new Dictionary<Combatant, int>();

    // Offensive → lowest HP enemy
    if ((skill.intents & IntentTag.Offensive) != 0)
    {
        foreach (var c in candidates)
        {
            int weight = 1;
            weight += (int)((1f - c.HPPercent) * 50);
            weights[c] = weight;

            Debug.Log($"{c.character.characterName} weight: {weight}");
        }
    }

    // Defensive → lowest HP ally
    else if ((skill.intents & IntentTag.Defensive) != 0)
    {
        foreach (var c in candidates)
        {
            int weight = 0;
            weight += (int)((1f - c.HPPercent) * 50);
            weights[c] = weight;

            Debug.Log($"{c.character.characterName} weight: {weight}");
        }
    }

    int totalWeight = 0;

    foreach (var pair in weights)
    {
        totalWeight += pair.Value;
    }

    int roll = Random.Range(0, totalWeight);
    Debug.Log($"{roll}");

    int current = 0;

    foreach (var pair in weights)
    {
        current += pair.Value;

        if (roll < current)
        {
            Debug.Log($"AI chose {pair.Key.character.characterName}");
            return new List<Combatant> { pair.Key };
        }
    }

    return new List<Combatant> { candidates[0] };
    }

}
