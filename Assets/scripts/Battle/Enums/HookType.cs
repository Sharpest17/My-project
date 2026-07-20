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

    OnKill,

    TurnStart,
    TurnEnd,
    
    SkillUsed,
    SkillTriggered,

    StatusModify,
    StatusApplied,
    StatusRefreshed,

    ModifySkillCost,

    ModifyResourceGain
}
