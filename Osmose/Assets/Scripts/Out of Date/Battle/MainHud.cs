using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that handles when Aren can use items in battle
/// </summary>
public class MainHud : MonoBehaviour {
    public Button ItemsButton;

    /// <summary>
    /// Set the Items button interactable
    /// </summary>
    public void SetItemsActive() {
        ItemsButton.interactable = true;
    }

    /// <summary>
    /// Set the Items button's interactable field depending on if Aren is shifted
    /// </summary>
    /// <param name="isShifted">True if Aren is shifted, false otherwise</param>
    public void ArenShifted(bool isShifted) {
        ItemsButton.interactable = !isShifted;
    }
}
