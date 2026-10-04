using UnityEngine;

[CreateAssetMenu(fileName = "Passive", menuName = "RPG/PassiveEffect/CloseSave")]
public class CloseSave : PassiveSkill
{
    public override void OnHook(HookType hook, CombatContext ctx)
    {
        if(hook == HookType.ModifyCombatStat)
        {
        CombatStatContext stat = ctx as CombatStatContext;


    if(stat == null)
        return;

    if(stat.attacker != owner)
        return;

    if(stat.stat == StatType.Evasion)
    {
        Debug.Log($"close save triggers, avo bonus is {owner.team.currentTP*3}");
        stat.value += owner.team.currentTP * 3;
    }
}
        if(hook == HookType.OnMiss && ctx.target == owner)
        {
            Debug.Log($"owner of close save dodged, gaining sp!");
            owner.GainResources(owner, 1, 0, false, true);
        }
        if(hook == HookType.ModifyResourceGain && ctx.target.IsAlly(owner))
        {
            ResourceContext recCtx = ctx as ResourceContext;
            Debug.Log($"recCtx null: {recCtx == null}");
Debug.Log($"target null: {recCtx?.target == null}");
Debug.Log($"owner null: {owner == null}");
Debug.Log($"owner team null: {owner?.team == null}");
            if(recCtx == null)
    return;
            if(owner.team.currentTP < 5)
            {
                recCtx.finalSP++;
                recCtx.finalTP++;
                Debug.Log("conditions met, final sp and tp gen increased");
            }
            
        }
    }
    }
