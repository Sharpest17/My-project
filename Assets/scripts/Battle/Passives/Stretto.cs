using UnityEngine;
using System.Collections.Generic;



[CreateAssetMenu(fileName = "Passive", menuName = "RPG/PassiveEffect/Stretto")]
public class Stretto : PassiveSkill
{
    public bool SkillAlreadyUsed = false;
    public Skill Enhanced;
    public int stacks;

    public float bonus;

    public List<Skill> skillsUsed = new List<Skill>();

    public override void OnHook(HookType hook, CombatContext ctx)
    {
        if(hook == HookType.UseSkill)
        {
            if (ctx is SkillContext skillCtx)
            {
                Skill usedSkill = skillCtx.skill;

                if (!skillsUsed.Contains(usedSkill)&&skillCtx.attacker == owner)
                {
                    skillsUsed.Add(usedSkill);
                    if(skillsUsed.Count >=3&& stacks <=3)
                    {
                        stacks++;
                        skillsUsed.Clear();
                    }
                }
            }
        }
        if(hook == HookType.SkillUsed)
        {
            if(ctx is SkillContext skillCtx)
            {
                if(skillCtx.skill == Enhanced &&(skillCtx.attacker == owner))
                {
                    SkillAlreadyUsed = true;
                }
            }
        }
        if(hook == HookType.ModifySkillCost)
        {
            if(ctx is CostContext costCtx)
            {
                if(costCtx.skill == Enhanced &&(costCtx.attacker == owner) && SkillAlreadyUsed)
                {
                    costCtx.baseSPMod-=3;
                    costCtx.tpMod -=3;
                }
            }
        }
        if(hook == HookType.ModifyStat)
        {
            if(ctx is StatContext statCtx)
            {
                if(statCtx.stat == StatType.Move&& (statCtx.attacker == owner))
                {
                statCtx.finalBonus += bonus*stacks;
                }

            }
        }
    }
}
