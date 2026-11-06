using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnOrderDisplay : MonoBehaviour
{
    public Image portrait;
    public TMP_Text characterName;
    public TMP_Text ticksRemaining;

    public void Setup(TicksTilTurn entry)
    {
        characterName.text =
            entry.combatant.character.characterName;

        ticksRemaining.text =
            $"{entry.ticksRemaining:0.#}";

        // Optional portrait assignment:
        // portrait.sprite = entry.combatant.character.portrait;
    }
}
