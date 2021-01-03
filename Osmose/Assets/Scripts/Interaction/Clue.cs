using UnityEngine;

[CreateAssetMenu(menuName = "Clue")]
public class Clue : ScriptableObject {
    [SerializeField] private string clueName;
    [SerializeField] private bool canUpdate;
    [SerializeField] private string description;
    [SerializeField] private string updatedDescription;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int clueNumber;

    /// <summary>
    /// Get the name of the clue
    /// </summary>
    /// <returns>Clue's name</returns>
    public string GetName() {
        return clueName;
    }

    /// <summary>
    /// Get the description of the clue
    /// </summary>
    /// <returns>Clue's description</returns>
    public string GetDescription() {
        if (canUpdate && CluesManager.Instance.DidUpdateClue(clueNumber)) {
            return updatedDescription;
        }
        return description;
    }

    /// <summary>
    /// Get the sprite of the description
    /// </summary>
    /// <returns>Clue's sprite</returns>
    public Sprite GetSprite() {
        return sprite;
    }

    /// <summary>
    /// Get the number of the clue
    /// </summary>
    /// <returns>Clue number</returns>
    public int GetClueNumber() {
        return clueNumber;
    }

    /// <summary>
    /// Determine whether or not the clue is equal to this clue
    /// </summary>
    /// <param name="other">Other clue that is being compared to this clue</param>
    /// <returns>True if the two clues are the same, false otherwise</returns>
    public bool IsEqual(Clue other) {
        return this.clueName.Equals(other.clueName) && this.clueNumber == other.clueNumber;
    }
}
