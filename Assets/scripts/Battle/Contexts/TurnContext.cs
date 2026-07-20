using UnityEngine;

public class TurnContext : CombatContext
{
    public bool extraTurn;

    public int turnCount;

    public TurnContext(Combatant actor, int turnCount, bool extraTurn = false)
    {
        this.attacker = actor;
        this.target = actor;
        this.turnCount = turnCount;
        this.extraTurn = extraTurn;
    }
}
