using UnityEngine;
using System;

[CreateAssetMenu(fileName = "GiveStatusEffect", menuName = "RPG/SkillEffect/GiveStatusEffect")]
public class GiveStatus : SkillEffect
{
    [Tooltip("Status to apply")]
    public StatusEffect statusPrefab; // assign a SO asset here

    [Range(0f, 1f)]
    public float chance = 1f;


    public override void Apply(Combatant user, Combatant target, Skill skill, SkillContext skillctx)
    {
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
