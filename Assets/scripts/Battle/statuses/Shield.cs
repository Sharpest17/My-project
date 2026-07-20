using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "RPG/StatusEffect/Shield")]
public class Shield : StatusEffect
{
    public int shieldValue;

    public StatType statType;

    public int originalShield;

    public override void OnHook(HookType hook, CombatContext ctx)
    {
        DamageContext dmg = ctx as DamageContext;
        StatusContext sts = ctx as StatusContext;


        if(dmg != null && hook == HookType.BeforeDamage && dmg.target == holder)
        {
            int remainingDamage = dmg.finalDamage - dmg.blockedDamage;

            if (remainingDamage <= 0) return;

            int absorbed = Mathf.Min(shieldValue, remainingDamage);

            dmg.blockedDamage += absorbed;
            shieldValue -= absorbed;

            Debug.Log($"shield has absorbed {dmg.blockedDamage}, shield value is now {shieldValue}, original shield was {originalShield}");

            if(shieldValue <= 0)
            {
                IsExpired = true;
            }
        }
        if (sts != null &&
    (hook == HookType.StatusApplied || hook == HookType.StatusRefreshed) &&
    sts.target == holder &&
    sts.status is Shield)
        {
            Shield newShield = sts.status as Shield;
            ShieldContext shd = new ShieldContext(
                sts.attacker, 
                sts.target,
                0,
                sts.status,
                newShield.statType
                );

            BattleMath.CalculateShielding(shd);

            if(shd.finalShield >= shieldValue && !shd.cancelled)
            {
                this.owner = shd.attacker;
                this.shieldValue = shd.finalShield;
                this.originalShield = shd.finalShield;
                this.statType = shd.scaling;
                Debug.Log($"shield value increased to {shieldValue}, original shield is now {originalShield}");
            }
        }
    }
}
