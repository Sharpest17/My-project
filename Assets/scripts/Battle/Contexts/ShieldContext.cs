using UnityEngine;

public class ShieldContext : CombatContext
{
    public StatType scaling;
    public int finalShield;

    public int baseShield;

    public int count;
    public int countMod = 0;

    public float outShield = 0f;
    public float inShield = 0f;

    public ShieldContext(Combatant shielder, Combatant target, int baseShield, StatusEffect status, StatType statType)
    {
        this.attacker = shielder;
        this.target = target;
        this.scaling = statType;
        this.count = status.count;
        this.baseShield = baseShield; //fixed value
        this.finalShield = baseShield; //fixed or calculated value
    }
}
