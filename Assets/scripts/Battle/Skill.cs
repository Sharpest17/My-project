using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Skill", menuName = "RPG/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public int power;
    public StatType attackingStat;
    public StatType defendStat;
    public TargetType targetType;

    public float actionCost;
    public int baseHit;
    public int baseCrit;
    public int spCost;
    public int tpCost;

    public SkillTag tags;
    public IntentTag intents;
    public DamageType damageType;
    public List<SkillEffect> effects;
    public virtual void Use(Combatant user, List<Combatant> targets, SkillContext skillctx)
    {
        foreach (var target in targets)
    {
        foreach (var effect in effects)
        {
            effect.Apply(user, target, this, skillctx);
        }
    }
    }
}
