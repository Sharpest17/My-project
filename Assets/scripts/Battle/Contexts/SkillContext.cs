using UnityEngine;

public class SkillContext : CombatContext
{
    public Skill skill;

    public float powerMod = 0f;

    public float hitMod = 0f;
    public float critMod = 0f;

    public bool lastHit;
    public bool lastCrit;
    public bool lastKOd;

    public bool lastHealCrit;

    public int actionValueMod = 0;

    public SkillContext(
        Combatant attacker,
        Skill skill)
    {
        this.attacker = attacker;
        this.skill = skill;
    }
}