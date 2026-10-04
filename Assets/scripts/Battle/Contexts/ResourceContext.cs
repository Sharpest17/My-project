using UnityEngine;

public class ResourceContext : CombatContext
{

    public int finalSP;
    public int finalTP;

    public bool spFixed;
    public bool tpFixed;

    public ResourceContext(
        Combatant source,
        Combatant receiver,
        int spGain,
        int tpGain,
        bool spFixed,
        bool tpFixed)
    {
        attacker = source;
        this.target = receiver;

        finalSP = spGain;
        finalTP = tpGain;

        this.spFixed = spFixed;
        this.tpFixed = tpFixed;
    }
}
