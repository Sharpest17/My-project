using UnityEngine;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Exploration,
        Battle
    }

    public GameState CurrentState;

    public List<Character> playerParty;

    private EncounterTrigger currentEncounter;

    public static GameManager Instance;

    public Encounter testEncounter;

    [Header("Managers")]
    public WorldManager worldManager;
    public BattleManager battleManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnterExploration();
    }

    void Awake()
{
    Instance = this;
}

    public void EnterExploration()
{
    CurrentState = GameState.Exploration;
    worldManager.enabled = true;
    battleManager.enabled = false;
    Debug.Log("Entered Exploration mode");
}

    public void EndEncounter(bool playersWon)
{
    if (playersWon && currentEncounter != null)
    {
        currentEncounter.gameObject.SetActive(false);
    }

    currentEncounter = null;

    EnterExploration();
}

public void EnterBattle(Encounter encounter, EncounterTrigger trigger)
{
    currentEncounter = trigger;

    CurrentState = GameState.Battle;
    worldManager.enabled = false;
    battleManager.enabled = true;

     battleManager.StartBattle(
        playerParty,
        encounter
    );
    Debug.Log("Entered Battle mode");
}

}
