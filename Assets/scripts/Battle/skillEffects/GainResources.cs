using UnityEngine;

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

    public override void Apply(
        Combatant user,
        Combatant target,
        Skill skill,
        SkillContext skillctx
        )
    {
        Combatant receiver =
            affectTarget
            ? target
            : user;

        receiver.GainResources(user, SPamount, TPamount, SPfixed, TPfixed);
    }
}
