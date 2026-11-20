using UnityEngine;

[CreateAssetMenu(fileName = "Passive", menuName = "RPG/PassiveEffect/Waagh")]
public class Waagh : PassiveSkill
{
    public override void OnHook(HookType hook, CombatContext ctx)
    {
        if(hook == HookType.ModifyStat && ctx is StatContext stat)
        {
            if(stat.attacker == owner && stat.stat == StatType.Strength)
            {
                stat.multiplier+= owner.team.currentTP/10f;
            }
        }
        if(ctx is BattleContext bat)
        {if(hook == HookType.BattleStart){
            BattleManager.Instance.AddMessage("WAAAGH! GANK EM LON!");
        }
        if(hook == HookType.BattleEnd)
        {
            if(bat.battle.victory){
            BattleManager.Instance.AddMessage("DA GANK GANG'S... BEEN GANKED...");
            }
                else
                {
                    BattleManager.Instance.AddMessage("BUNCHA KRUMPS GIT CHUMPED!");
                }
        }
        }

        
    }
}
