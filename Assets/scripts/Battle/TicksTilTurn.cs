using UnityEngine;

public class TicksTilTurn
{
    public Combatant combatant;

    public float ticksRemaining;

    public TicksTilTurn(Combatant combatant, float ticksRemaining)
    {
        this.combatant = combatant;
        this.ticksRemaining = ticksRemaining;
    }
}
