using UnityEngine;

public class DamageContext : CombatContext
{

    public Skill skill;

    public int finalDamage;
    public StatType attackStat;
    public StatType defenseStat;
    public float hitMod;
    public float evadeMod;
    public float critMod;
    public float critAvoMod;

    public DamageSource source;
    public DamageType type;

    public float skillPower;

    public float critBonus = 0.5f;
    public float damagePercent = 0f;
    public float vulnerability = 0f;
    public float resistance = 0f;
    public float variance = 0f;

    public float bonusDamage = 0f;

    public int blockedDamage = 0;

    public bool critical;
    public bool dodged;
    public bool denied;
    public bool autoHit;
    public bool autoCrit;

    public DamageContext(Combatant attacker, Combatant target, int baseDamage,
                         DamageSource source, DamageType type, StatType attack = StatType.Strength, StatType defense = StatType.Defense, Skill skill = null)
    {
        this.attacker = attacker;
        this.target = target;
        this.finalDamage = baseDamage;
        this.skillPower = baseDamage;
        this.attackStat = attack;
        this.defenseStat = defense;
        this.source = source;
        this.type = type;
        this.skill = skill;
        this.dodged = false;
        this.cancelled = false;
        this.denied = false;
        this.autoHit = false;
        this.autoCrit = false;
    }
}
