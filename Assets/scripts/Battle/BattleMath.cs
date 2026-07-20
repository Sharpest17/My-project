using UnityEngine;
public static class BattleMath
{
    public static void CalculateDamage(DamageContext ctx)
    {
        // 1. Base stat scaling
        BattleManager.Instance.modifierManager.Broadcast(HookType.ModifyDamage, ctx);

        int attackStat = ctx.attacker.GetModifiedStat(ctx.attackStat);
        int defenseStat = ctx.target.GetModifiedStat(ctx.defenseStat);

        float baseHit = ctx.attacker.GetModifiedCombatStat(StatType.Accuracy);
        float baseEvasion = ctx.target.GetModifiedCombatStat(StatType.Evasion);
        float baseCrit = ctx.attacker.GetModifiedCombatStat(StatType.CritChance);
        float baseCritAvoid = ctx.target.GetModifiedCombatStat(StatType.CritAvoid);
        Debug.Log($"Base Hit: {baseHit}");
        Debug.Log($"Base Evasion: {baseEvasion}");
        Debug.Log($"Base Crit: {baseCrit}");
        Debug.Log($"Base Crit Avoid: {baseCritAvoid}");

        float baseResist = ctx.target.GetResistance(ctx.type);


        //will be replaced with the proper formulas for hit and crit rates later
        float finalHit = (baseHit+ctx.hitMod)-(baseEvasion+ctx.evadeMod);
        float finalCrit = (baseCrit+ctx.critMod) -(baseCritAvoid+ctx.critAvoMod);
        Debug.Log($"final hit rate is {finalHit}");
        Debug.Log($"final crit rate is {finalCrit}");

        if (!ctx.autoHit)
        {
        float hitRoll = Random.Range(0f, 100f);
        Debug.Log($"hit roll is {hitRoll}");
        if (hitRoll> finalHit)
        {
            ctx.cancelled = true;
            ctx.dodged = true;
        }
        }
        ctx.critical = ctx.autoCrit || (Random.Range(0f, 100f) < finalCrit);

        float critFactor = ctx.critical ? (1f + ctx.critBonus) : 1f;
        float dmgFactor = 1f + ctx.damagePercent;
        float vulnFactor = 1f + ctx.vulnerability;
        float resistFactor = 1f - (baseResist + ctx.resistance);
        float varianceFactor = 1f + ctx.variance;

        float final = 
            (attackStat *
            ctx.skillPower *
            critFactor *
            dmgFactor *
            vulnFactor *
            resistFactor *
            varianceFactor)
            /
            (Mathf.Max(0, defenseStat)+5f)
            +ctx.bonusDamage;

        ctx.finalDamage = Mathf.RoundToInt(final);


        // 5. Clamp
        if (ctx.finalDamage <= 0)
            ctx.finalDamage = 1;
    }

    public static void CalculateHealing(HealContext ctx)
    {
        BattleManager.Instance.modifierManager.Broadcast(HookType.ModifyHeal, ctx);

        int scaling = ctx.attacker.GetModifiedStat(ctx.healStat);
        float baseCrit = ctx.attacker.GetModifiedCombatStat(StatType.CritChance);
        float baseCritAvoid = ctx.target.GetModifiedCombatStat(StatType.CritAvoid);
        float finalCrit = (baseCrit+ctx.critMod) -(baseCritAvoid+ctx.critAvoMod);
        ctx.critical = ctx.autoCrit || (Random.Range(0f, 100f) < finalCrit);

        float critFactor = ctx.critical ? (1f + ctx.critBonus) : 1f;

        float finalHeal = 
        (scaling
        * (ctx.basePower+ctx.powerMod)
        * (1+ctx.outHeal)
        * (1+ctx.inHeal)
        * (critFactor))
               / 10f;

        ctx.finalHeal = Mathf.RoundToInt(finalHeal);

        if (ctx.finalHeal < 1)
            ctx.finalHeal = 1;
    }

    public static void CalculateShielding(ShieldContext ctx)
    {
        BattleManager.Instance.modifierManager.Broadcast(HookType.ModifyShield, ctx);

        int scaling = ctx.attacker.GetModifiedStat(ctx.scaling);
        Debug.Log($"Shield calc by: {ctx.attacker.character.characterName}");
        
        float finalShield =
        (scaling
        *(1+ctx.outShield)
        *(1+ctx.inShield)
        *(1+.2f*(ctx.count+ctx.countMod))
        );
        Debug.Log($"count: {ctx.count+ctx.countMod+5}, scaling stat: {ctx.scaling}, scaling stat value: {scaling}");

        ctx.finalShield = Mathf.RoundToInt(finalShield);
    }
}