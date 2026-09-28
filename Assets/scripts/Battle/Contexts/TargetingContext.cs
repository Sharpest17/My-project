using System.Collections.Generic;

public class TargetingContext : CombatContext
{
    public Skill skill;

    public List<Combatant> validTargets;
    
   public List<Combatant> forcedTargets =
        new List<Combatant>();

    public List<Combatant> hiddenTargets =
        new List<Combatant>();
        
    public TargetingContext(
        Combatant user,
        Skill skill,
        List<Combatant> validTargets
    )
    {
        this.attacker = user;
        this.skill = skill;
        this.validTargets = validTargets;
    }
}