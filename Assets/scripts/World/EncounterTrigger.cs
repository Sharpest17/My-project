using UnityEngine;

public class EncounterTrigger : MonoBehaviour
{

    public Encounter encounter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.EnterBattle(encounter, this);
        }
    }
}
