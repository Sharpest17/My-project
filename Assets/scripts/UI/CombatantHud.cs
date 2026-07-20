using TMPro;
using UnityEngine;

public class CombatantHud : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text hpText;
    public TMP_Text spText;

    private Combatant combatant;

    public void Setup(Combatant c)
    {
        combatant = c;
        
        nameText.text = $"{combatant.character.characterName}";

        Refresh();
    }

    public void Refresh()
    {
        hpText.text =
        $"{combatant.currentHP}/" +
        $"{combatant.GetModifiedStat(StatType.MaxHP)}";

        spText.text =
        $"{combatant.currentSP}/"+
        $"{combatant.GetModifiedStat(StatType.MaxSP)}";
    }
}