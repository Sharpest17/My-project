using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "RPG/StatusEffect/DOT")]
public class DOT : StatusEffect
{
    public int damage;

    public override void OnHook(HookType hook, CombatContext ctx)
    {
        StatusContext sts = ctx as StatusContext;
        if((hook == HookType.StatusApplied || hook == HookType.StatusRefreshed) && ctx.target == holder&& sts.status.Name == "Sickness")
        {
            DamageContext dmg = new DamageContext(
                owner,              // attacker (the applier)
                holder,             // target (poisoned unit)
                damage+count+(owner.GetModifiedStat(StatType.Magic))/2,
                DamageSource.Status,
                DamageType.Neutral
            );
            BattleManager.Instance.QueueDamage(dmg);

            /*HealContext heal = new HealContext(
                owner,
                holder,
                10,
                StatType.Attack,
                null
            );
            BattleManager.Instance.QueueHealing(heal);*/
        }

        if(hook == HookType.TurnEnd && ctx.attacker == holder)
        {
            HealContext heal = new HealContext(
                owner,
                holder,
                (holder.GetModifiedStat(StatType.MaxHP))/10,
                StatType.Magic,
                null
            );
            BattleManager.Instance.QueueHealing(heal);
        }
    }
}
