using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that handles the indication of character selection in battle
/// </summary>
public class PartyUI : MonoBehaviour {
    public Color RegularColor; // regular color of party member ui
    public Color HighlightedColor; // highlighted color of party member ui

    private Image image; // image of the part member ui

    // Start is called before the first frame update
    void Start() {
        image = this.GetComponent<Image>();
    }

    /// <summary>
    /// Highlight or de-highlight the character
    /// </summary>
    /// <param name="selected">Flag if the character is being selected</param>
    public void Highlight(bool selected) {
        if (selected) {
            // hightlight
            image.color = HighlightedColor;
        } else {
            // dehighlight
            image.color = RegularColor;
        }
    }
}
