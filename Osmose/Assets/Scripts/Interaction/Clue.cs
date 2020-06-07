using UnityEngine;

[CreateAssetMenu(menuName = "Clue")]
public class Clue : ScriptableObject {
    [SerializeField] private string clueName;
    [SerializeField] private string description;
    [SerializeField] private Sprite sprite;

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
        return description;
    }

    /// <summary>
    /// Get the sprite of the description
    /// </summary>
    /// <returns>Clue's sprite</returns>
    public Sprite GetSprite() {
        return sprite;
    }
}
