using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMember : MonoBehaviour
{
    public Color RegularColor; // regular color of party member ui
    public Color HighlightedColor; // highlighted color of party member ui

    private Image image; // image of the part member ui

    // Start is called before the first frame update
    void Start() {
        image = this.GetComponent<Image>();
    }

    public void Highlight(bool selected) {
        if (selected) {
            image.color = RegularColor;
        } else {
            image.color = HighlightedColor;
        }
    }
}
