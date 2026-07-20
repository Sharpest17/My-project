using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class Character : ScriptableObject
{
    public string characterName;
    public bool isPlayerControlled;

    public int baseMaxHP;
    public int baseStrength;
    public int baseMagic;
    public int baseDefense;
    public int baseResistance;
    public int baseSpeed;
    public int baseSkill;
    public int baseLuck;
    public int baseMove;

    public int maxSP;

    public List<Skill> skills;

    public List<PassiveSkill> passives;
}