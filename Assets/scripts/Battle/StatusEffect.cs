using UnityEngine;
using System.Collections.Generic;

public abstract class StatusEffect : CombatModifier
{
    public string Name;
    public Combatant holder;
    public int baseDuration;
    public int duration;
    public DurationType durationType;
    public int count;
    public bool IsExpired;
}
