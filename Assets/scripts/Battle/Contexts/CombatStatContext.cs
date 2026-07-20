using UnityEngine;

public class CombatStatContext : CombatContext
{
    public StatType stat;
    public float value;

    public CombatStatContext(Combatant owner, StatType stat, float value)
    {
        attacker = owner;
        target = owner;
        this.stat = stat;
        this.value = value;
    }
    
}