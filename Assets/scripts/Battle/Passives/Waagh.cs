using UnityEngine;

[CreateAssetMenu(fileName = "Passive", menuName = "RPG/PassiveEffect/Waagh")]
public class Waagh : PassiveSkill
{
    public override void OnHook(HookType hook, CombatContext ctx)
    {
        if(hook == HookType.ModifyStat && ctx is StatContext stat)
        {
            if(stat.attacker == owner && stat.stat == StatType.Strength)
            {
                stat.multiplier+= owner.team.currentTP/10f;
            }
        }
    }
}
