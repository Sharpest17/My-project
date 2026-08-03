using UnityEngine;

public class HealContext : CombatContext
{
    public StatType healStat;
    public float critMod;
    public float critAvoMod;
    public int basePower;

    public int powerMod;
    public float outHeal = 0f;
    public float inHeal = 0f;
    public float critBonus = .5f;
    public int finalHeal;

    public Skill skill;

    public bool critical;
    public bool autoCrit;

    public bool canCrit;

    public HealContext(Combatant healer, Combatant target, int setHeal, StatType statType = StatType.Magic, Skill skill = null)
    {
        this.attacker = healer;
        this.target = target;
        this.healStat = statType;
        this.basePower = setHeal;
        this.finalHeal = setHeal; //primarily set for fixed heal values that don't scale
        this.skill = skill;
    }
}
