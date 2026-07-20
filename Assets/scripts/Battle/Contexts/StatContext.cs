using UnityEngine;

public class StatContext : CombatContext
{
    public StatType stat;

    public float baseValue;
    public float baseBonus;
    public float multiplier;

    public StatContext(Combatant owner, StatType stat, int baseValue)
    {
        this.attacker = owner;
        this.target = owner;
        this.stat = stat;
        this.baseValue = baseValue;
        this.baseBonus = 0f;
        this.multiplier = 0f;
    }

    public float GetFinal()
    {
        return (baseValue + baseBonus) * (1 + multiplier);
    }
}