using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Passive", menuName = "RPG/PassiveEffect/TenonPassive")]
public class TenonPassive : PassiveSkill
{
    public Skill pMain;
    public Character Scrap;
    public StatusEffect Scrapping;
    public Character Hammer;
    public Character Healbot;
    public Skill heal;
    public StatusEffect bonus;

    public override void OnHook(HookType hook, CombatContext ctx)
    {
        if((hook == HookType.UnitEnters||hook == HookType.UnitRemoved) && ctx is UnitContext unit)
        {
            if(unit.unit.character == Hammer&& unit.unit.IsAlly(owner))
            {
                owner.GainResources(owner, 0, 1, true, false);
            }
        }
        if(hook == HookType.OnHit && ctx is DamageContext dmg&&dmg.attacker == owner)
        {
            Debug.Log("you hit something");
            if(dmg.skill == pMain)
            {
                if(dmg.target.IsAlly(owner))
                {
                    if (dmg.target.character == Scrap)
                        {
                        StatusEffect newStatus = Instantiate(Scrapping);
                        StatusContext stat = new StatusContext(owner, dmg.target, newStatus);

                        BattleManager.Instance.modifierManager.Broadcast(HookType.StatusModify, ctx);
                        if (stat.cancelled)
                        {
                        return;
                        }
                        dmg.target.ApplyStatus(stat);
                        BattleManager.Instance.AddMessage("you hit a scrap bot, it's locking on");
                        }
                    else if (dmg.target.character == Hammer)
                        {
                        owner.GainResources(owner, 0, 1, true, false);
                        }
                    else if (dmg.target.character == Healbot)
                        {
                    List<Combatant> targets =
                        BattleManager.Instance.GetAllAliveAllies(dmg.target);

                        BattleManager.Instance.TriggerSkill(dmg.target, heal, targets);
                        }
                    else
                        {
                        StatusEffect newStatus = Instantiate(bonus);
                        StatusContext stat = new StatusContext(owner, dmg.target, newStatus);

                        BattleManager.Instance.modifierManager.Broadcast(HookType.StatusModify, ctx);
                        if (stat.cancelled)
                        {
                        return;
                        }
                        dmg.target.ApplyStatus(stat);
                        BattleManager.Instance.AddMessage($"{dmg.target.character.characterName} used their head for once");
                        }
                }
            }
        }
    }
}
