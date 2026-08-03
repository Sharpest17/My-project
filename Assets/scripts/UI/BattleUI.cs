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

    public Transform allyPanel;
    public Transform enemyPanel;

    public Transform tpPanel;
    public GameObject teamHudPrefab;

    private List<CombatantHud> combatantDisplays = new List<CombatantHud>();
    
    private List<TeamHud> teamDisplays = new List<TeamHud>();

    public GameObject combatantDisplayPrefab;
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
        battleManager.OnTargetsSelected(
        new List<Combatant> { target }
        );
        });
    }
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

    combatantDisplays.Clear();

    skillPanel.gameObject.SetActive(false);
    targetPanel.gameObject.SetActive(false);
}
}