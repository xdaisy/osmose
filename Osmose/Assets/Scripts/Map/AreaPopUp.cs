using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Class that handles the Area Popup UI changes
/// </summary>
public class AreaPopUp : MonoBehaviour {
    public Text[] Areas;

    private EventSystem eventSystem;

    private void Awake() {
        eventSystem = EventSystem.current;
    }

    /// <summary>
    /// Open up Select Area pop up
    /// </summary>
    /// <param name="areas">List of areas within a region</param>
    public void OpenPopUp(List<string> areas) {
        for (int i = 0; i < Areas.Length; i++) {
            if (i >= areas.Count || !EventManager.Instance.DidEventHappened(areas[i])) {
                // if there is no more areas or if the player hasn't gone to the area yet
                // set area button inactive
                Areas[i].gameObject.SetActive(false);
                continue;
            }
            // make sure that the area button is active
            Areas[i].gameObject.SetActive(true);
            string area = areas[i].Replace('_', ' ');
            Areas[i].text = area;
        }

        eventSystem.SetSelectedGameObject(Areas[0].gameObject);
    }
}
