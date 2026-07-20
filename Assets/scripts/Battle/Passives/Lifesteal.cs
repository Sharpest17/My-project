using UnityEngine;

[CreateAssetMenu(fileName = "Passive", menuName = "RPG/PassiveEffect/Lifesteal")]
public class Lifesteal : PassiveSkill
{
    public override void OnHook(HookType hook, CombatContext ctx)
    {
        if(hook == HookType.AfterDamage && ctx.attacker == owner)
        {
            Debug.Log("lifesteal should trigger");
            DamageContext dmg = ctx as DamageContext;
            HealContext heal = new HealContext(owner, owner, 0, StatType.Strength, null);
            heal.finalHeal = dmg.finalDamage/2;
            BattleManager.Instance.QueueHealing(heal);
        }
    }
}
