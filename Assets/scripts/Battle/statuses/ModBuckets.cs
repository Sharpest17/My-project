using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "RPG/StatusEffect/ModBuckets")]
public class ModBuckets : StatusEffect
{
    [Tooltip("whether it affects incoming damage/heal/shield or outgoing")]
    public bool incoming;
    public bool outgoing;

    [Tooltip("damage modifier values")]
    public float hitMod;
    public float evadeMod;
    public float critMod;
    public float critAvoMod;
    public float critBonus;
    public float damagePercent;
    public float vulnerability;
    public float resistance;
    public bool dodged;
    public bool denied;
    public bool autoHit;
    public bool autoCrit;
    public float bonusDamage;

    [Tooltip("heal modifiers")]
    public float outHeal;
    public float inHeal;
    public float healCritMod;
    public float healCritAvoMod;
    public int healPowerMod;
    public bool HealAutoCrit;
    public bool HealCanCrit;
    public int bonusHeal;


    [Tooltip("shield modifiers")]
    public int countMod;
    public float outShield;
    public float inShield;


    public override void OnHook(
    HookType hook,
    CombatContext ctx)
    {
        switch (hook)
    {
        case HookType.ModifyDamage:
            if (ctx is DamageContext dmg)
            {
                if((incoming && dmg.target ==holder) ||(outgoing && dmg.attacker == holder))
                    {
                        dmg.hitMod += hitMod;
                        dmg.evadeMod += evadeMod;
                        dmg.critMod += critMod;
                        dmg.critAvoMod += critAvoMod;
                        dmg.critBonus += critBonus;
                        dmg.damagePercent += damagePercent;
                        dmg.vulnerability += vulnerability;
                        dmg.resistance += resistance;
                        if(dodged)
                        {
                            dmg.dodged = true;
                            dmg.cancelled = true;
                        }
                        if(denied)
                        {
                            dmg.denied = true;
                            dmg.cancelled = true;
                        }
                        if(autoHit)
                        {
                            dmg.autoHit = true;
                        }
                        if(autoCrit)
                        {
                            dmg.autoCrit = true;
                        }
                        dmg.bonusDamage += bonusDamage;
                    }
            }
            break;

        case HookType.ModifyHeal:
            if (ctx is HealContext healCtx)
            {
            if((incoming && healCtx.target ==holder) ||(outgoing && healCtx.attacker == holder)){
                healCtx.critMod += healCritMod;
                healCtx.critAvoMod += healCritAvoMod;
                healCtx.powerMod += healPowerMod;
                healCtx.bonusHeal += bonusHeal;
                
                if(HealCanCrit)
                    {
                        healCtx.canCrit = true;
                    }
                if(HealAutoCrit)
                    {
                        healCtx.autoCrit = true;
                    }
                    if(healCtx.target==holder)
                    {
                        healCtx.inHeal += inHeal;
                    }
                    if(healCtx.attacker == holder)
                    {
                        healCtx.outHeal += outHeal;
                    }
            }
            }
            break;

        case HookType.ModifyShield:
            if (ctx is ShieldContext shieldCtx)
            {
                if((incoming && shieldCtx.target ==holder) ||(outgoing && shieldCtx.attacker == holder))
                {
                shieldCtx.countMod += countMod;
                if(shieldCtx.target == holder)
                        {
                            shieldCtx.inShield += inShield;
                        }
                if(shieldCtx.attacker == holder)
                        {
                            shieldCtx.outShield += outShield;
                        }
            }
            }
            break;
    }
    }
}
