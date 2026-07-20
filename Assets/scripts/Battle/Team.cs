using UnityEngine;

public class Team
{
    public string name;
    public int currentTP;

    public int maxTP;
    public Team(string teamName, int startingTP = 0, int maximumTP = 10)
    {
        name = teamName;
        currentTP = startingTP;
        maxTP = maximumTP;
    }
}
