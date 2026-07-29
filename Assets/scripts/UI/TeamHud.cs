using TMPro;
using UnityEngine;

public class TeamHud : MonoBehaviour
{
    public TMP_Text teamNameText;
    public TMP_Text tpText;

    private Team team;

    public void Setup(Team team)
    {
        this.team = team;
        Refresh();
    }

    public void Refresh()
    {
        teamNameText.text = team.name;
        tpText.text = $"TP: {team.currentTP}/{team.maxTP}";
    }
}