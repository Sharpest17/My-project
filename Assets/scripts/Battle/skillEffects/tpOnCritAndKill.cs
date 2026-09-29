using UnityEngine;

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
    
    public override void Apply(Combatant user, Combatant target, Skill skill, SkillContext skillctx)
    {
          Combatant receiver =
            affectTarget
            ? target
            : user;
            
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
        
            receiver.GainResources(user, tally*sp, tally*tp, spFixed, tpFixed);
    }
}
