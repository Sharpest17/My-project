using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    public GameManager gameManager;
    public bool battleActive = false;
    public Combatant currentCombatant;

    public List<Team> teams = new List<Team>();
    private Skill pendingSkill;

    public BattleUI battleUI;
    public List<Combatant> combatants;
    public static BattleManager Instance;

    public ModifierManager modifierManager;

    int turnCount = 0;
    private float BaseActionCost = 100f;
    Queue<DamageContext> damageQueue = new Queue<DamageContext>();
    Queue<HealContext> healingQueue = new Queue<HealContext>();

    public Skill basic;

    public Skill fireBall;

    public Skill guard;
    public Skill bigGuard;

    public Skill plagueStrike;

    public Skill cure;

    public PassiveSkill lifesteal;
    
public void EndBattle(bool playersWon)
    {
        if (playersWon)
        {
        Debug.Log("Victory! Reward player.");
    }
        else
           {
        Debug.Log("Defeat! Return to last checkpoint.");
    }
    // Reset battle state if needed
        currentCombatant = null;
        gameManager.EnterExploration();
    }

 void Awake()
    {
        Instance = this;
    }

/*public void SimulateBattle()
{
    // Create characters
    Character hero = new Character("Hero", true, 20, 10, 2, 11);
    Character warrior = new Character("Warrior", true, 30, 8, 3, 10);
    Character wizard = new Character("Wizard", false, 60, 5, 1, 9);
    Character cleric = new Character("Cleric", false, 60, 6, 2, 15);
    Character dummy = new Character("Dummy", false, 5, 0, 2, 5);

    hero.skills.Add(basic);
    hero.skills.Add(bigGuard);
    warrior.skills.Add(plagueStrike);
    wizard.skills.Add(bigGuard);
    wizard.skills.Add(fireBall);
    cleric.skills.Add(guard);
    cleric.skills.Add(basic);

    wizard.passives.Add(lifesteal);

        // Assign skills to the character
    // Wrap as combatants
    Combatant c1 = new Combatant(hero);
    Combatant c2 = new Combatant(warrior);
    Combatant c3 = new Combatant(wizard);
    Combatant c4 = new Combatant(cleric);
    Combatant c5 = new Combatant(dummy);

    c1.resistances[DamageType.Fire] = .5f;
    //c2.passives.Add(new TheBleeding());
    //c4.passives.Add(new LIvely());

    combatants = new List<Combatant> { c2, c1, c3, c4};

    //StartBattle();
}*/

public void StartBattle(
    List<Character> playerParty,
    Encounter encounter)
{
    combatants = new List<Combatant>();
    Team playerTeam = new Team("player team");
    Team enemyTeam = new Team("enemy team");

    teams.Add(playerTeam);
    teams.Add(enemyTeam);
    playerTeam.currentTP = 5;
    playerTeam.currentTP = 3;

    foreach (var ally in playerParty)
    {
        combatants.Add(
            new Combatant(ally)
        );
    }

    foreach (var enemy in encounter.enemies)
    {
        combatants.Add(
            new Combatant(enemy)
        );
    }

    battleActive = true;

    modifierManager = new ModifierManager(combatants);

    foreach (var combatant in combatants)
{
    combatant.currentSP = combatant.GetModifiedStat(StatType.MaxSP);
    combatant.currentHP = combatant.GetModifiedStat(StatType.MaxHP);
    if(combatant.isPlayerControlled)
            {
                combatant.team = playerTeam;
            }
            else
            {
                combatant.team = enemyTeam;
            }

    Debug.Log($"{combatant.character.name} is currently on the {combatant.team.name}");
}
    foreach (var combatant in combatants)
{
    if (!combatant.isPlayerControlled)
    {
        combatant.ai = new EnemyAI(this, combatant.profile);
    }
}
    foreach (var combatant in combatants)
{
    combatant.actionValue = BaseActionCost / combatant.GetModifiedCombatStat(StatType.ActionSpeed);
}
    battleUI.SetupCombatHUD(combatants);
    ProcessTurns();
}
public void StartTurn(Combatant current)
{
    currentCombatant = current;
    if (combatants.Count == 0)
        return;
    TurnContext ctx = new TurnContext(current, turnCount);
    turnCount++;
    current.TickStatuses(DurationType.TurnStart);
    ExecutePhase(HookType.TurnStart, ctx);
    ClearAllStatuses();

    if (!current.IsAlive())
    {
        Debug.Log($"{current.character.characterName} is currently down, skipped");
        EndTurn(current);
        return;
    }

    //foreach (var c in combatants)
    //{
    //Debug.Log($"{c.character.characterName} AV: {c.actionValue}");
    //}

    if(ctx.cancelled){
        Debug.Log($"{current.character.characterName} cannot act.");
        EndTurn(current);
        return;
    }

    if (current.isPlayerControlled)
{
    battleUI.ShowSkills(current);
    return;
}

    Skill chosenSkill = current.ChooseSkill(this);

     if (chosenSkill == null)
    {
        Debug.Log("no skills available, what a dummy!");
        EndTurn(current);
        return;
    }

    List<Combatant> targets = current.ChooseTargets(this, chosenSkill);
    
    UseSkill(current, chosenSkill, targets);
}

private void UseSkill(Combatant user, Skill skill, List<Combatant> targets)
{
    //user.SpendMana(skill.manaCost);
    //user.TriggerCooldown(skill);

    skill.Use(user, targets);
    user.SpendResources(skill);
    BattleManager.Instance.ProcessDamageQueue();
    BattleManager.Instance.ProcessHealingQueue();
    battleUI.RefreshCombatHUD();
    user.actionValue += skill.actionCost / user.GetModifiedCombatStat(StatType.ActionSpeed);

    CheckBattleEnd();
    EndTurn(user);
}

public void TriggerSkill(Combatant user, Skill skill, Combatant target)
{
    List<Combatant> targets = new List<Combatant> { target };

    foreach (var t in targets)
    {
        foreach (var effect in skill.effects)
        {
            effect.Apply(user, t, skill);
        }
    }
    BattleManager.Instance.ProcessDamageQueue();
    BattleManager.Instance.ProcessHealingQueue();
    battleUI.RefreshCombatHUD();

    CheckBattleEnd();
}

public void EndTurn(Combatant combatant)
{
    combatant.actionValue += BaseActionCost/combatant.GetModifiedCombatStat(StatType.ActionSpeed);
    TurnContext ctx = new TurnContext(combatant, turnCount);
    combatant.TickStatuses(DurationType.TurnEnd);
    ExecutePhase(HookType.TurnEnd, ctx);
    ClearAllStatuses();
    if (!battleActive)
        return;
    ProcessTurns();
}

public void AdvanceTurn()
{
    float smallestAV = float.MaxValue;
    foreach (var combatant in combatants)
    {
        if (!combatant.IsAlive())
            continue;

        if (combatant.actionValue < smallestAV)
        {
            smallestAV = combatant.actionValue;
        }
    }
    foreach(var combatant in combatants)
        {
            combatant.actionValue = combatant.actionValue - smallestAV;
        }
}

public void ProcessTurns()
{
    AdvanceTurn();
    
    foreach (var combatant in combatants)
    {
        if (combatant.actionValue <= 0)
        {
            StartTurn(combatant);
            return;
        }
    }
}

public void ClearAllStatuses()
    {
        foreach(var combatant in combatants)
        {
            combatant.ClearStatuses();
        }
    }

    private void CheckBattleEnd()
{
    bool anyPlayersAlive =
        combatants.Any(c =>
            c.IsPlayerControlled() &&
            c.IsAlive());

    bool anyEnemiesAlive =
        combatants.Any(c =>
            !c.IsPlayerControlled() &&
            c.IsAlive());

    if (!anyPlayersAlive || !anyEnemiesAlive)
    {
        battleActive = false;

        bool playersWon = anyPlayersAlive;
        battleUI.ClearCombatHUD();
        GameManager.Instance.EndEncounter(playersWon);
    }
}

    public void QueueDamage(DamageContext ctx)
{
    Debug.Log("Queued damage: " + ctx.finalDamage);
    damageQueue.Enqueue(ctx);
}

    public void QueueHealing(HealContext ctx)
{
    healingQueue.Enqueue(ctx);
}

    public void ProcessDamageQueue()
{
    while (damageQueue.Count > 0)
    {
        DamageContext ctx = damageQueue.Dequeue();
        ctx.target.TakeDamage(ctx);
    }
    battleUI.RefreshCombatHUD();
}

    public void ProcessHealingQueue()
{
    while (healingQueue.Count > 0)
    {
        HealContext ctx = healingQueue.Dequeue();

        ctx.target.RestoreHP(ctx);
    }
    battleUI.RefreshCombatHUD();
}

    public void ExecutePhase(HookType hook, CombatContext ctx)
    {
    modifierManager.Broadcast(hook, ctx);
    ProcessDamageQueue();
    ProcessHealingQueue();
    battleUI.RefreshCombatHUD();
    }

    public void OnSkillSelected(Skill skill)
{
    pendingSkill = skill;

    var targets =
        GetValidTargets(
            currentCombatant,
            skill
        );

    battleUI.ShowTargets(targets);
}

    public void OnTargetsSelected(
    List<Combatant> targets)
{
    Skill skilltoUse = pendingSkill;
    pendingSkill = null;
    UseSkill(
        currentCombatant,
        skilltoUse,
        targets
    );
}


    public List<Combatant> GetValidTargets(Combatant user, Skill skill)
    {
        List<Combatant> targets = new List<Combatant>();
        switch (skill.targetType)
    {
        case TargetType.SingleEnemy:
        {
                        targets.AddRange(GetAllAliveEnemies(user));
            break;
        }

        case TargetType.AllEnemies:
        {
            targets.AddRange(GetAllAliveEnemies(user));
            break;
        }

        case TargetType.SingleAlly:
        {
            
                        targets.AddRange(GetAllAliveAllies(user));
            break;
        }

        case TargetType.Self:
        {
            targets.Add(user);
            break;
        }

        case TargetType.RandomEnemy:
        {
            Combatant enemy = GetRandomAliveEnemy(user);
            if (enemy != null)
                targets.Add(enemy);
            break;
        }

        case TargetType.RandomAlly:
        {
            Combatant ally = GetRandomAliveAlly(user);
            if (ally != null)
                targets.Add(ally);
            break;
        }

        case TargetType.AllAllies:
        {
            targets.AddRange(GetAllAliveAllies(user));
            break;
        }

        case TargetType.DeadAlly:
        {
            Combatant ally = GetFirstDeadAlly(user);
            if (ally != null)
            targets.Add(ally);
            break;
        }   
    }
        return targets;
    }

    private Combatant GetFirstAliveEnemy(Combatant user)
    {
    return combatants.Find(c => 
        c.IsAlive() && !c.IsAlly(user));
    }

    private List<Combatant> GetAllAliveEnemies(Combatant user)
    {
    return combatants.FindAll(c => 
        c.IsAlive() && !c.IsAlly(user));
    }

    private Combatant GetFirstAliveAlly(Combatant user)
    {
    return combatants.Find(c => 
        c.IsAlive() && c.IsAlly(user));
    }

    private Combatant GetRandomAliveEnemy(Combatant user)
    {
    List<Combatant> enemies = GetAllAliveEnemies(user);

    if (enemies.Count == 0)
        return null;

    return enemies[UnityEngine.Random.Range(0, enemies.Count)];
    }

    private Combatant GetRandomAliveAlly(Combatant user)
    {
    List<Combatant> allies = GetAllAliveAllies(user);

    if (allies.Count == 0)
        return null;

    return allies[UnityEngine.Random.Range(0, allies.Count)];
    }

    private List<Combatant> GetAllAliveAllies(Combatant user)
    {
    return combatants.FindAll(c =>
        c.IsAlive() && c.IsAlly(user));
    }
    private Combatant GetFirstDeadAlly(Combatant user)
{
    return combatants.Find(c =>
        !c.IsAlive() &&
        c.IsAlly(user));
}
}
