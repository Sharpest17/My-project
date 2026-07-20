using UnityEngine;

public class StatusContext : CombatContext
{
    public StatusEffect status;

    public int count;
    public int duration;

    public float chanceMod;
    public bool witScaling;

    public bool dexScaling;
    public bool mightScaling;

    public StatusContext(Combatant applier, Combatant target, StatusEffect status)
    {
        this.attacker = applier;
        this.target = target;
        this.count = status.count;
        this.duration = status.baseDuration;
        this.status = status;
        this.chanceMod = 0f;
    }
}
