using UnityEngine;

[CreateAssetMenu(fileName = "Passive", menuName = "RPG/PassiveEffect/ViolentStride")]

public class ViolentStride : PassiveSkill
{
    public float CritMod;
    public float CritBonusMod;
    public override void OnHook(HookType hook, CombatContext ctx)
    {
        if(hook == HookType.ModifyDamage && ctx is DamageContext dmg)
        {
            if(dmg.attacker == owner || dmg.target == owner)
            {
                Debug.Log($"bonus to crit is {CritMod}, bonus to crit damage is {CritBonusMod}");
                dmg.critMod += CritMod;
                dmg.critBonus += CritBonusMod;
            }
        }
    }
}
