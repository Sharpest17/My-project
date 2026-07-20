using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Combatant
{
    public Character character;
    public bool isPlayerControlled;
    public EnemyAI ai;

    public Team team;

    public AIProfile profile;

    // Battle-state values
    public int currentHP;

    public int currentSP;
    public int initiative;
    public bool hasActed;

    public float actionValue;
    [SerializeField] private int baseStrength;
    [SerializeField] private int baseMagic;
    [SerializeField] private int baseDefense;
    [SerializeField] private int baseResistance;
    [SerializeField] private int baseSpeed;
    [SerializeField] private int baseSkill;
    [SerializeField] private int baseLuck;
    [SerializeField] private int baseMove;
    [SerializeField] private int baseMaxHP;
    [SerializeField] private int baseMaxSP;

    public List<Skill> skills => character.skills;
    public List<StatusEffect> statusEffects;

    public Dictionary<DamageType, float> resistances;

    public List<PassiveSkill> passives;

    public Combatant(Character character)
    {
        this.character = character;
        statusEffects = new List<StatusEffect>();
        baseStrength = character.baseStrength;
        baseMagic = character.baseMagic;
        baseDefense = character.baseDefense;
        baseResistance = character.baseResistance;
        baseSpeed = character.baseSpeed;
        baseSkill = character.baseSkill;
        baseLuck = character.baseLuck;
        baseMove = character.baseMove;
        baseMaxHP = character.baseMaxHP;
        baseMaxSP = character.maxSP;
        isPlayerControlled = character.isPlayerControlled;
        resistances = new Dictionary<DamageType, float>();
        foreach (DamageType type in System.Enum.GetValues(typeof(DamageType)))
        {
            resistances[type] = 0f;
        }
        passives = new List<PassiveSkill>();

        foreach (var passive in character.passives)
        {
            PassiveSkill instance = Object.Instantiate(passive);
            instance.owner = this;
            passives.Add(instance);
        }
        hasActed = false;
    }

    public int GetModifiedStat(StatType stat)
    {
    int value;

    switch (stat)
    {
        case StatType.Strength: value = baseStrength; break;
        case StatType.Magic: value = baseMagic; break;
        case StatType.Defense: value = baseDefense; break;
        case StatType.Resistance: value = baseResistance; break;
        case StatType.Speed: value = baseSpeed; break;
        case StatType.Skill: value = baseSkill; break;
        case StatType.Luck: value = baseLuck; break;
        case StatType.Move: value = baseMove; break;
        case StatType.MaxHP: value = baseMaxHP; break;
        case StatType.MaxSP: value = baseMaxSP; break;
        default: return 0;
    }

    //Debug.Log($"Requested combat stat: {stat}, base value is {value}");

    StatContext ctx = new StatContext(this, stat, value);

    BattleManager.Instance.modifierManager.Broadcast(HookType.ModifyStat, ctx);

    return Mathf.RoundToInt(ctx.GetFinal());
    }

    public float GetModifiedCombatStat(StatType stat)
    {
    float value;

    switch (stat)
    {
        case StatType.Accuracy: value = (GetModifiedStat(StatType.Skill)*1.5f)+GetModifiedStat(StatType.Luck); break;
        case StatType.Evasion: value = GetModifiedStat(StatType.Speed)+(GetModifiedStat(StatType.Skill)/2f); break;
        case StatType.CritChance: value = (GetModifiedStat(StatType.Skill)/2f)+GetModifiedStat(StatType.Luck); break;
        case StatType.CritAvoid: value = GetModifiedStat(StatType.Luck); break;
        case StatType.ActionSpeed: value = GetModifiedStat(StatType.Move)+(GetModifiedStat(StatType.Speed)/2); break;
        default: return 0;
    }
    //Debug.Log($"Requested combat stat: {stat}, base value is {value}");

    CombatStatContext ctx = new CombatStatContext(this, stat, value);

    BattleManager.Instance.modifierManager.Broadcast(HookType.ModifyCombatStat, ctx);

    return ctx.value;
    }

    public float GetResistance(DamageType type)
    {
    if (resistances.TryGetValue(type, out float value))
        return value;

    return 0f;
    }

    public CostContext GetFinalCost(Skill skill)
{
    CostContext ctx =
        new CostContext(this, skill);

    BattleManager.Instance.modifierManager.Broadcast(
        HookType.ModifySkillCost,
        ctx
    );

    ctx.finalSPCost =
    Mathf.RoundToInt(
        (skill.spCost + ctx.baseSPMod)
        * (1+ctx.spMultiplier)
    )
    + ctx.finalSPMod;

    ctx.finalTPCost =
        skill.tpCost + ctx.tpMod;

    if (ctx.noSPCost)
        ctx.finalSPCost = 0;

    if (ctx.noTPCost)
        ctx.finalTPCost = 0;

    ctx.finalSPCost = Mathf.Max(0, ctx.finalSPCost);
    ctx.finalTPCost = Mathf.Max(0, ctx.finalTPCost);
    return ctx;
}



    public float HPPercent
{
    get
    {
        if (GetModifiedStat(StatType.MaxHP) <= 0) return 0f;
        return (float)currentHP / GetModifiedStat(StatType.MaxHP);
    }
}

    public List<CombatModifier> GetAllModifiers()
    {
    List<CombatModifier> all = new List<CombatModifier>();
    all.AddRange(statusEffects);
    all.AddRange(passives);
    return all;
    }

    public void TakeDamage(DamageContext ctx)
{
    if(ctx.denied)
        {
            return;
        }
    if (ctx.dodged)
        {
            Debug.Log($"{ctx.target.character.characterName} dodged!");
            return;
        }
    
    bool wasAlive = IsAlive();
    
    BattleManager.Instance.modifierManager.Broadcast(HookType.BeforeDamage, ctx);
    //applies damage
    currentHP -= (ctx.finalDamage- ctx.blockedDamage);
    if (currentHP < 0)
        currentHP = 0;

    Debug.Log($"{this.character.characterName} has taken {ctx.finalDamage} from {ctx.attacker.character.characterName}'s {ctx.source}!");

    BattleManager.Instance.modifierManager.Broadcast(HookType.AfterDamage, ctx);

    if (wasAlive && !IsAlive())
        {
        Debug.Log($"{this.character.characterName} is down");
        BattleManager.Instance.modifierManager.Broadcast(HookType.OnKill, ctx);
        }
}

    public void RestoreHP(HealContext ctx)
    {
        int before = currentHP;
    currentHP += ctx.finalHeal;

    if (currentHP > GetModifiedStat(StatType.MaxHP))
        currentHP = GetModifiedStat(StatType.MaxHP);

        Debug.Log($"{ctx.attacker.character.characterName} healed {ctx.finalHeal} HP to {character.characterName}!");
        Debug.Log($"current hp is now {currentHP}, max hp is {GetModifiedStat(StatType.MaxHP)}");
        
    }

    public void SpendResources(Skill skill)
    {
        CostContext cost =
        GetFinalCost(skill);

        currentSP -= cost.finalSPCost;
        Debug.Log($"current sp is now {currentSP}");
        team.currentTP -= cost.finalTPCost;
        Debug.Log($"current tp is now {team.currentTP}");
    }

    public void ApplyStatus(StatusContext ctx)
    {
    ctx.status.owner = ctx.attacker;
    ctx.status.holder = this;
    StatusEffect exists = statusEffects.Find(s => s.Name == ctx.status.Name);

            if(exists != null)
            {
                bool refreshed = true;
                if (ctx.status.count > exists.count)
                {
                    exists.count = ctx.status.count;
                }

                // Update duration
                if (ctx.status.duration > exists.duration)
                {
                    exists.duration = ctx.status.duration;
                }

                if(refreshed)
                {
                    //Debug.Log($"Refreshed status {exists.Name} on {character.characterName}");
                    BattleManager.Instance.modifierManager.Broadcast(HookType.StatusRefreshed, ctx);
                }
                BattleManager.Instance.ProcessDamageQueue();
                BattleManager.Instance.ProcessHealingQueue();
                //Debug.Log($"status already present, New duration: {exists.duration}, New stack count: {exists.count}");
                //Debug.Log($"Statuses now: {this.statusEffects.Count}");
                return;
            }

            //Debug.Log($"Applying status {ctx.status.Name} to {this.character.characterName}");
            statusEffects.Add(ctx.status);
            BattleManager.Instance.modifierManager.Broadcast(HookType.StatusApplied, ctx);
            BattleManager.Instance.ProcessDamageQueue();
            BattleManager.Instance.ProcessHealingQueue();
            //Debug.Log($"Statuses now: {this.statusEffects.Count}");

    }

    public void TickStatuses(DurationType currentTick)
    {
        foreach(var status in statusEffects)
        {
            if(status.durationType == currentTick)
            {
                status.duration--;
                if(status.duration <= 0)
            {
                status.IsExpired = true;
                //Debug.Log($"the {status.Name} status has expired");
            }
            }
        }
    }

    public void ClearStatuses()
    {
        foreach(var status in statusEffects.ToList())
        {
            if(status.IsExpired)
            {
                //broadcast for status removal here
                this.statusEffects.Remove(status);
            }
        }
    }
    public Skill ChooseSkill(BattleManager battle)
{
    Debug.Log($"{character.characterName}'s turn");
    if (IsPlayerControlled())
        return ChoosePlayerSkill(battle);

    return ChooseAISkill(battle);
}

    public List<Skill> GetAvailableSkills()
{
    return character.skills
        .Where(skill => CanUseSkill(skill))
        .ToList();
}

public bool CanUseSkill(Skill skill)
{
    CostContext cost =
    GetFinalCost(skill);
    if(currentSP < cost.finalSPCost)
        {
            Debug.Log($"{skill.name}'s sp cost too high, skill is not usable");
            return false;
        }
    if(team.currentTP < cost.finalTPCost)
        {
            Debug.Log($"{skill.name}'s tp cost too high, skill is not usable");
            return false;
        }
    //if (IsOnCooldown(skill))
    //    return false;

    //if (IsSilenced() && skill.HasTag(SkillTag.Spell))
      //  return false;

    return true;
}

private Skill ChoosePlayerSkill(BattleManager battle)
{
    return GetAvailableSkills().FirstOrDefault();
}

private Skill ChooseAISkill(BattleManager battle)
{
    var available = GetAvailableSkills();

    if (available.Count == 0){
        return null;
    }

    if (ai != null)
    {
        return ai.ChooseSkill(this, battle); // <-- only returns Skill
    }
    //Debug.Log("no ai present, picking at random");

    return available[Random.Range(0, available.Count)];
}

public List<Combatant> ChooseTargets(BattleManager battle, Skill skill)
{
    var validTargets = battle.GetValidTargets(this, skill);

    if (validTargets.Count == 0)
        return validTargets;

    // AI-controlled
    if (!isPlayerControlled && ai != null)
    {
        return ai.ChooseTargets(this, skill, validTargets);
    }

    // Player-controlled (temporary fallback)
    return validTargets;
}

    public bool IsAlive()
    {
    return currentHP > 0;
    }

    public bool IsAlly(Combatant b)
    {
        return this.IsPlayerControlled() == b.IsPlayerControlled();
    }
    public bool IsPlayerControlled()
    {
    return isPlayerControlled;
    }
}
