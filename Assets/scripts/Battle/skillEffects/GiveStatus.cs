using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GiveStatusEffect", menuName = "RPG/SkillEffect/GiveStatusEffect")]
public class GiveStatus : SkillEffect
{
    [Tooltip("Status to apply")]
    public StatusEffect statusPrefab; // assign a SO asset here

    [Range(0f, 1f)]
    public float chance = 1f;

    public bool affectTarget;

    public override void Apply(Combatant user, List<Combatant> targets, Skill skill, SkillContext skillctx)
    {
        if(affectTarget)
        {
            foreach(Combatant target in targets){
            StatusEffect newStatus = Instantiate(statusPrefab);
            StatusContext ctx = new StatusContext(user, target, newStatus);

            BattleManager.Instance.modifierManager.Broadcast(HookType.StatusModify, ctx);

            if (UnityEngine.Random.value >= chance)
        {
            Debug.Log($"{user.character.characterName} {newStatus.Name} missed!");
            return;
        }
            if (ctx.cancelled)
        {
            Debug.Log($"{user.character.characterName} {newStatus.Name} was denied!");
            return;
        }
            target.ApplyStatus(ctx);
            }
        }
        else
        {
            StatusEffect newStatus = Instantiate(statusPrefab);
            StatusContext ctx = new StatusContext(user, user, newStatus);

            BattleManager.Instance.modifierManager.Broadcast(HookType.StatusModify, ctx);

            if (UnityEngine.Random.value >= chance)
        {
            Debug.Log($"{user.character.characterName} {newStatus.Name} missed!");
            return;
        }
            if (ctx.cancelled)
        {
            Debug.Log($"{user.character.characterName} {newStatus.Name} was denied!");
            return;
        }
            user.ApplyStatus(ctx);
        }
    }
}
