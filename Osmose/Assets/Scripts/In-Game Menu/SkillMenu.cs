using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillMenu : MonoBehaviour
{
    [Header("Character Select Panel")]
    public CanvasGroup CharSelectPanel;
    public Button[] Characters;

    private string currChar;

    // Start is called before the first frame update
    void Start()
    {
        currChar = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenSkillMenu() {
        CharSelectPanel.interactable = true;
        List<string> party = GameManager.Instance.Party.GetCurrentParty();
        for (int i = 0; i < Characters.Length; i++) {
            Text charName = Characters[i].GetComponentInChildren<Text>();
            if (i >= party.Count) {
                // no party member
                charName.text = "";
                Characters[i].interactable = false;
                continue;
            }
            charName.text = party[i];
        }

        EventSystem.current.SetSelectedGameObject(Characters[0].gameObject);
        currChar = party[0];
    }
}
