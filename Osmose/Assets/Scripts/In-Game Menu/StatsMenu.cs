using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatsMenu : MonoBehaviour
{
    private EventSystem eventSystem;

    public CanvasGroup CharacterSelect;
    public Button[] Characters;

    private void Awake() {
        eventSystem = EventSystem.current;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenStatsMenu() {
        CharacterSelect.interactable = true;

        // show the name of the current party members
        List<string> currentParty = GameManager.Instance.Party.GetCurrentParty();
        for (int i = 0; i < Characters.Length; i++) {
            Text name = Characters[i].GetComponentInChildren<Text>();
            if (i >= currentParty.Count) {
                // no other party member
                name.text = "";
                Characters[i].interactable = false;
            } else {
                name.text = currentParty[i];
                Characters[i].interactable = true;
            }
        }
        eventSystem.SetSelectedGameObject(Characters[0].gameObject);
    }
}
