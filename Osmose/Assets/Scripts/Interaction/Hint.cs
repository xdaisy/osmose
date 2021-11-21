using UnityEngine;

[CreateAssetMenu(menuName = "Hint")]
public class Hint : ScriptableObject {
    [SerializeField] private Clue clue;
    [SerializeField] bool canUpdate;
    [SerializeField] private string[] dialogue;
    [SerializeField] private string[] updateDialogue;

    public Clue GetClue() {
        return clue;
    }

    public bool getCanUpdate() {
        return canUpdate;
    }

    public string[] GetDialogue() {
        return dialogue;
    }

    public string[] GetUpdateDialogue() {
        return updateDialogue;
    }
}
