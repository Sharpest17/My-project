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

        ResourceContext ctx =
            new ResourceContext(
                user,
                receiver,
                SPamount,
                TPamount,
                SPfixed,
                TPfixed
            );

        BattleManager.Instance.modifierManager.Broadcast(
            HookType.ModifyResourceGain,
            ctx
        );

        receiver.currentSP += ctx.finalSP;
        receiver.team.currentTP += ctx.finalTP;

        receiver.currentSP =
            Mathf.Clamp(
                receiver.currentSP,
                0,
                receiver.GetModifiedStat(StatType.MaxSP)
            );

        receiver.team.currentTP =
            Mathf.Clamp(
                receiver.team.currentTP,
                0,
                receiver.team.maxTP
            );

        Debug.Log(
            $"{receiver.character.characterName} gained " +
            $"{ctx.finalSP} SP and {ctx.finalTP} TP"
        );
    }
}
