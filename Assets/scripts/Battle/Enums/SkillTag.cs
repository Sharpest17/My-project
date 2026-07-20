using System;
[Flags]
public enum SkillTag
{
    none = 0,
    slash = 1 << 0,
    pierce = 1 << 1,
    bludgeon = 1 << 2,
    weapon = 1 << 3,
    ranged = 1 << 4,
    elemental = 1 << 5,
    psychic = 1 << 6,
    divine = 1 << 7,
    reason = 1 << 8,
    food = 1 << 9,
    poison = 1 << 10
}