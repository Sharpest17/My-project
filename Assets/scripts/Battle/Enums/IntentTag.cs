using UnityEngine;
using System;

[Flags]
public enum IntentTag
{
    None = 0,
    Offensive = 1 << 0, //anything with intent to damage or amplify damage
    Defensive = 1 << 1, //anything with intent to protect or sustain allies
    Supportive = 1 << 2, //anything with intent to support via buffs, action advancing, etc
    Disruptive = 1 << 3 //anything with intent to disrupt via debuffs, action delaying, etc
}
