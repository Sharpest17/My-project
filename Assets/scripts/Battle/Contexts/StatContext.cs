using UnityEngine;

public class StatContext : CombatContext
{
    public StatType stat;

    public float baseValue;
    public float baseBonus;
    public float multiplier;
    public float finalBonus;

    public StatContext(Combatant owner, StatType stat, float baseValue)
    {
        this.attacker = owner;
        this.target = owner;
        this.stat = stat;
        this.baseValue = baseValue;
        this.baseBonus = 0f;
        this.multiplier = 0f;
        this.finalBonus = 0f;
    }

    public float GetFinal()
    {
        return (baseValue + baseBonus) * (1 + multiplier)+finalBonus;
    }
}