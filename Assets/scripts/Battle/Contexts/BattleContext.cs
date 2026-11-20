using UnityEngine;

public class BattleContext : CombatContext
{
    public BattleManager battle;
    public BattleContext(BattleManager battle)
    {
        this.battle = battle;
    }
}
