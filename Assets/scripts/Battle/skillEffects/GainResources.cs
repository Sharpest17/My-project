using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu( fileName = "GainResources", menuName = "RPG/SkillEffect/GainResources")]
public class GainResources : SkillEffect
{
    [Header("Resource Gain")]
    public int SPamount;
    public int TPamount;

    [Header("Modification Rules")]
    public bool SPfixed;
    public bool TPfixed;

    [Header("Targeting")]
    public bool affectTarget;

    public override void Apply(Combatant user, List<Combatant> targets, Skill skill, SkillContext skillctx)
    {
        HashSet<Team> affectedTeams = new HashSet<Team>();

        if(affectTarget){
            foreach (Combatant target in targets)
    {

        if (affectedTeams.Add(target.team))
        {
            target.GainResources(
                user,
                SPamount,
                TPamount,
                SPfixed,
                TPfixed
            );
        }
                else
                {
                    target.GainResources(
                    user,
                    SPamount,
                    0,
                    SPfixed,
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
                SPamount,
                TPamount,
                SPfixed,
                TPfixed
            );
        }
    }
}
