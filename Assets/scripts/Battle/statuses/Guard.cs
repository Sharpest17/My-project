using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "RPG/StatusEffect/Guard")]
public class Guard : StatusEffect
{
    public float resist;

    public int Thorns;

    public override void OnHook(HookType hook, CombatContext ctx)
    {
        if (hook == HookType.ModifyDamage)
        {
            DamageContext dmg = ctx as DamageContext;

            if (dmg == null)
                return;
            
            if(dmg.attacker == owner &&dmg.source == DamageSource.Skill)
            {
                dmg.damagePercent+=1f;
                return;
            }

            // Only reduce damage taken by the guarded unit
            if (dmg.target != owner)
                return;

            dmg.resistance+= resist;
            dmg.evadeMod += .25f;
        }
        if(hook == HookType.ModifyStat && ctx.attacker != owner && ctx.attacker.IsAlly(owner))
        {
            StatContext stat = ctx as StatContext;
            if(stat.stat == StatType.Strength)
            {
                stat.multiplier = stat.multiplier+.5f;
            }
        }

        if(hook == HookType.AfterDamage && ctx.target == holder)
        {
            DamageContext dmg1 = ctx as DamageContext;
            if(dmg1.source == DamageSource.Skill){
            DamageContext dmg = new DamageContext(
                holder,              // attacker (the applier)
                ctx.attacker,             // target (poisoned unit)
                Thorns,
                DamageSource.Status,
                DamageType.Neutral
            );

            BattleManager.Instance.modifierManager.Broadcast(HookType.ModifyDamage, dmg);
            Debug.Log($"{dmg.target.character.characterName} has taken {dmg.finalDamage} damage from Thorns!");
            dmg1.target.TakeDamage(dmg1);
            }
        }
    }
}
