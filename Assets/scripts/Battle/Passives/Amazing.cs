using UnityEngine;

[CreateAssetMenu(fileName = "Passive", menuName = "RPG/PassiveEffect/Amazing")]
public class Amazing : PassiveSkill
{
    private int charge;
    private int threshold = 3;

    public Skill enhanced;
    public override void OnHook(HookType hook, CombatContext ctx)
    {
        if(hook == HookType.OnCrit && ctx.attacker == owner)
        {
            DamageContext dmg = ctx as DamageContext;

            if(dmg.skill != enhanced)
            {
                charge++;
                Debug.Log($"charge increased, charge is now {charge}");
            }
        }

        if(hook == HookType.ModifySkillCost && ctx.attacker == owner)
        {
            CostContext costCtx = ctx as CostContext;

            if(costCtx.skill == enhanced && charge >= threshold)
            {
                costCtx.noSPCost = true;
                costCtx.noTPCost = true;
            }
        }
        if((hook == HookType.UseSkill|| hook == HookType.SkillTriggered)&& ctx.attacker == owner)
        {
            SkillContext skillctx = ctx as SkillContext;
            
            if(skillctx.skill == enhanced && charge >= threshold)
            {
                charge -= threshold;
            }
        }
    }
}
