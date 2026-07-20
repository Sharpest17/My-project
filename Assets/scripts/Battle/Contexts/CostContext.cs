using UnityEngine;

public class CostContext : CombatContext
{
    public Skill skill;
    public int baseSPMod;
    public float spMultiplier;
    public int finalSPMod;

    public int tpMod;

    public bool noSPCost;
    public bool noTPCost;

    public int finalSPCost;
    public int finalTPCost;

    public CostContext(Combatant owner, Skill skill)
    {
        this.attacker = owner;
        this.skill = skill;
    }
}
