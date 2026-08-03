public enum HookType
{
    ModifyStat,
    ModifyCombatStat,

    ModifyDamage,
    ModifyHeal,
    ModifyShield,

    BeforeDamage,
    AfterDamage,

    BeforeHeal,
    AfterHeal,

    OnCrit,
    OnMiss,
    OnHit,
    OnKill,

    TurnStart,
    TurnEnd,
    
    UseSkill,
    SkillTriggered,

    StatusModify,
    StatusApplied,
    StatusRefreshed,

    ModifySkillCost,

    ModifyResourceGain
}
