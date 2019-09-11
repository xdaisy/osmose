using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AreaPopUp : MonoBehaviour {
    public Text[] Areas;

    private EventSystem eventSystem;

    private void Awake() {
        eventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update() {
        //eventSystem = EventSystem.current;
    }

    /// <summary>
    /// Open up Select Area pop up
    /// </summary>
    /// <param name="areas">List of areas within a region</param>
    public void OpenPopUp(List<string> areas) {
        for (int i = 0; i < Areas.Length; i++) {
            if (i >= areas.Count) {
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
