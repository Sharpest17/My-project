using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "resourceOnCritsKills", menuName = "RPG/SkillEffect/resourceOnCritsKills")]
public class resourceOnCritsKills : SkillEffect
{
    [Header("tp and if it's fixed")]
    public int tp;
    public bool tpFixed;

    [Header("sp and if it's fixed")]
    public int sp;
    public bool spFixed;

    [Header("Targeting")]
    public bool affectTarget;

    [Header("crits or ko's?")]
    public bool crits;
    public bool KOs;
    
    public override void Apply(Combatant user, List<Combatant> targets, Skill skill, SkillContext skillctx)
    {       
        int tally = 0;

            if(crits)
        {
            tally+= skillctx.critCount;
        }
            if(KOs)
        {
            tally+= skillctx.koCount;
        }
            Debug.Log($"total crits {skillctx.critCount}, total kos {skillctx.koCount}, total tally {tally}");
        
        int spAmount = tally * sp;
        int tpAmount = tally * tp;

            HashSet<Team> affectedTeams = new HashSet<Team>();


        if(affectTarget){
            foreach (Combatant target in targets)
    {

        if (affectedTeams.Add(target.team))
        {
            target.GainResources(
                user,
                spAmount,
                tpAmount,
                spFixed,
                tpFixed
            );
        }
                else
                {
                    target.GainResources(
                    user,
                    spAmount,
                    0,
                    spFixed,
                    true   
                    );
                }
    }
    }
        else
        {
            user.GainResources
            (
                user,
                spAmount,
                tpAmount,
                spFixed,
                tpFixed
            );
        }
    }
}
