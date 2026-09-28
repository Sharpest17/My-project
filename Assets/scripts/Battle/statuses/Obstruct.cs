using UnityEngine;


[CreateAssetMenu(
fileName = "Obstruct",
menuName = "RPG/StatusEffect/Obstruct")]
public class Obstruct : StatusEffect
{
    public override void OnHook(
    HookType hook,
    CombatContext ctx
)
    {
        if(hook == HookType.ModifyTargets)
{
    TargetingContext targetCtx =
        ctx as TargetingContext;

    if(targetCtx == null || targetCtx.skill.targetType != TargetType.SingleEnemy)
        return;

        Debug.Log($"status owner is {this.owner.character.characterName}");
        Debug.Log($"status holder is {this.holder.character.characterName}");

        if(ctx.attacker != holder)
            {
                return;
            }
        
        if(targetCtx.validTargets.Contains(owner))
    {
        if(!targetCtx.forcedTargets.Contains(owner))
        {
            Debug.Log($"{this.owner.character.characterName} is a forced target to {owner.character.characterName}");
            targetCtx.forcedTargets.Add(owner);
        }
    }
}
    }
}