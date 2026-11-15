using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "Passive", menuName = "RPG/PassiveEffect/Bladesworn")]
public class Bladesworn : PassiveSkill
{
    public int oath = 0;
    public Skill triggerable;

    public override void OnHook(HookType hook, CombatContext ctx)
    {
        if(hook == HookType.TurnStart && ctx is TurnContext turn)
        {
            if(turn.attacker == owner)
            {
                oath++;
                switch(oath)
                {
                case 1: BattleManager.Instance.AddMessage("Lon: i swear upon steel"); break;
                case 2: BattleManager.Instance.AddMessage("Lon: i swear upon the blade"); break;
                case 3: BattleManager.Instance.AddMessage("Lon: i swear upon the knife"); break;
                case 4: BattleManager.Instance.AddMessage("Lon: i swear upon what was cut"); break;
                case 5: BattleManager.Instance.AddMessage("Lon: i swear upon what is cut"); break;
                case 6: BattleManager.Instance.AddMessage("Lon: i swear upon what will be cut"); break;
                case 7: BattleManager.Instance.AddMessage("Lon: ... i left my sword in the ship"); 
              
                //BattleManager.Instance.TriggerSkill(owner, triggerable, targets);
                oath = 0;
                break;
                }
            }
        }
    }
}
