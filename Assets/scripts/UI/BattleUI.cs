using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BattleUI : MonoBehaviour
{
    public BattleManager battleManager;

    public Transform skillPanel;
    public Transform targetPanel;

    public GameObject skillButtonPrefab;

    public GameObject turnDisplayPrefab;

    public Transform allyPanel;
    public Transform enemyPanel;

    public Transform tpPanel;
    public Transform TurnPanel;

    public Transform LogPanel;
    public GameObject teamHudPrefab;

    private List<CombatantHud> combatantDisplays = new List<CombatantHud>();
    
    private List<TeamHud> teamDisplays = new List<TeamHud>();

    public GameObject combatantDisplayPrefab;

    public GameObject combatLogPrefab;

    public UIState currentState;

    public Combatant currentCombatant;  

    public Skill selectedSkill;
    public List<Combatant> selectedTargets = new List<Combatant>();

    public void ShowSkills(Combatant player)
{
    skillPanel.gameObject.SetActive(true);
    ClearPanel(skillPanel);
    foreach (var skill in player.skills)
    {
        CostContext cost =
        player.GetFinalCost(skill);
        GameObject button =
            Instantiate(skillButtonPrefab, skillPanel);
        button.GetComponentInChildren<TMP_Text>().text =
    $"{skill.skillName} " +
    $"({cost.finalSPCost} SP/{cost.finalTPCost} TP)";

        Button uiButton = button.GetComponent<Button>();

        bool usable = player.CanUseSkill(skill);
        uiButton.interactable = usable;

        uiButton.onClick.AddListener(() =>
        {
            Debug.Log($"Selected {skill.skillName}");
            skillPanel.gameObject.SetActive(false);
            selectedSkill =skill;
            battleManager.OnSkillSelected(skill);
        });
        // set button text
    }
}
    public void ShowTargets(List<Combatant> targets)
    {
        //check for if it's an aoe skill, if it is, set one button to select all targets
        targetPanel.gameObject.SetActive(true);
        ClearPanel(targetPanel);
        foreach (var target in targets)
    {
        GameObject button =
            Instantiate(skillButtonPrefab, targetPanel);

        button.GetComponentInChildren<TMP_Text>().text =
            target.character.characterName;
        Button uiButton = button.GetComponent<Button>();

        uiButton.onClick.AddListener(() =>
{
        Debug.Log($"Selected target: {target.character.characterName}");
        targetPanel.gameObject.SetActive(false);
        selectedTargets.Add(target);
        foreach (var combatant in selectedTargets)
        {
            Debug.Log(combatant.character.characterName);
        }
        battleManager.OnTargetsSelected(
        new List<Combatant> { target }
        );
        });
    }
        GameObject cancelButton = 
            Instantiate(skillButtonPrefab, targetPanel);
            cancelButton.GetComponentInChildren<TMP_Text>().text = "cancel";
        Button cancel = cancelButton.GetComponent<Button>();
        cancel.onClick.AddListener(() =>
{
        Debug.Log($"target selection cancelled");
        CancelSelection();
        });
    }

    public void UpdateTurnOrder(List<TicksTilTurn> turnOrder)
{
    ClearPanel(TurnPanel);

    foreach (TicksTilTurn entry in turnOrder)
    {
        Debug.Log(
            $"{entry.combatant.character.characterName}: " +
            $"{entry.ticksRemaining:F1} ticks remaining"
        );
        GameObject turnDisplay = Instantiate(turnDisplayPrefab, TurnPanel);
        TurnOrderDisplay display =
            turnDisplay.GetComponent<TurnOrderDisplay>();
        
        display.Setup(entry);
        // Instantiate turn order display
        // Set portrait/name
    }
}
    public void CancelSelection()
{
    selectedSkill = null;
    selectedTargets.Clear();

    currentState = UIState.skillselection;

    ClearPanel(targetPanel);
    ShowSkills(currentCombatant);
}
    public void DisplayLogs()
    {
        ClearPanel(LogPanel);
        if(battleManager.combatLog.Count >0)
        {
            string message = battleManager.combatLog.Dequeue();
            GameObject Log = Instantiate(combatLogPrefab, LogPanel);
            Log.GetComponentInChildren<TMP_Text>().text = message;
            Button continueButton = Log.GetComponent<Button>();
            continueButton.onClick.AddListener(() =>
        {
            OnMessageContinue();
        });
        }
        else
        {
            battleManager.ContinueTurn();
        }
    }
    public void OnMessageContinue()
{
    DisplayLogs();
}

private void ClearPanel(Transform panel)
{
    foreach (Transform child in panel)
    {
        Destroy(child.gameObject);
    }
}

    public void SetupCombatHUD(
    List<Combatant> combatants)
{
    ClearPanel(allyPanel);
    ClearPanel(enemyPanel);

    combatantDisplays.Clear();

    foreach (var combatant in combatants)
    {
        Transform parent =
            combatant.isPlayerControlled
            ? allyPanel
            : enemyPanel;

        GameObject display =
            Instantiate(
                combatantDisplayPrefab,
                parent
            );

        CombatantHud hud =
            display.GetComponent<CombatantHud>();

        hud.Setup(combatant);

        combatantDisplays.Add(hud);
    }
}
    public void SetupTPHUD(List<Team> teams)
{
    Debug.Log($"team count {teams.Count}");
    ClearPanel(tpPanel);
    teamDisplays.Clear();

foreach (Team team in teams)
{
    GameObject display =
        Instantiate(teamHudPrefab, tpPanel);

    TeamHud hud =
        display.GetComponent<TeamHud>();

    hud.Setup(team);

    teamDisplays.Add(hud);
}
}

    public void RefreshCombatHUD()
{
    foreach (var hud in combatantDisplays)
    {
        hud.Refresh();
    }

    foreach (var hud in teamDisplays)
    {
        hud.Refresh();
    }
}
    public void ClearCombatHUD()
{
    ClearPanel(allyPanel);
    ClearPanel(enemyPanel);
    ClearPanel(tpPanel);
    ClearPanel(TurnPanel);
    ClearPanel(LogPanel);

    combatantDisplays.Clear();

    skillPanel.gameObject.SetActive(false);
    targetPanel.gameObject.SetActive(false);
}
}