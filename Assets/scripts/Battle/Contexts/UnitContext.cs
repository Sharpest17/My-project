using UnityEngine;

public class UnitContext : CombatContext
{
    public Combatant unit;
    
    public UnitContext(Combatant unit)
    {
        this.unit = unit;
    }
}
