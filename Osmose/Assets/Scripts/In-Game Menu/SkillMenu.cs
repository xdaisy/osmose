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

    [Header("Skills List")]
    public CanvasGroup SkillsPanel;
    public Text[] Skills;

    [Header("Description")]
    public Text Description;

    private string currChar; // keep track of the current character whose skill looking at
    private int skillIndx; // keep track of where in the character's skill list

    private int currSkillIndx; // keep track of which skill was clicked
    private string currSkill; // keep track of current skill cursor is on

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (CharSelectPanel.interactable && Input.GetButtonDown("Horizontal")) {
            // if selecting character and detect key, set the current char to current highlighted character
            currChar = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
            updateSkillList();
        }
    }

    public void OpenSkillMenu() {
        CharSelectPanel.interactable = true;
        List<string> party = GameManager.Instance.Party.GetCurrentParty();
        for (int i = 0; i < Characters.Length; i++) {
            Text charName = Characters[i].GetComponentInChildren<Text>();
            if (i >= party.Count) {
                // no party member
                Characters[i].gameObject.SetActive(false);
                continue;
            }
            charName.text = party[i];
        }

        EventSystem.current.SetSelectedGameObject(Characters[0].gameObject);
        currChar = party[0];
        updateSkillList();
    }

    public void CloseSkillMenu() {
        skillIndx = 0;
        currChar = "";
    }

    // open skills panel
    public void OpenSkillsPanel() {
        CharSelectPanel.interactable = false;
        SkillsPanel.interactable = true;
        EventSystem.current.SetSelectedGameObject(Skills[0].gameObject);
        currSkill = Skills[0].text;
        updateDescription();
    }

    // closes skills panel
    public void CloseSkillsPanel() {
        Description.text = "";
        currSkill = "";

        SkillsPanel.interactable = false;
        CharSelectPanel.interactable = true;

        EventSystem.current.SetSelectedGameObject(null);
        // set pointer to character whose skills was looking at
        for (int i = 0; i < Characters.Length; i++) {
            if (Characters[i].GetComponentInChildren<Text>().text == currChar) {
                EventSystem.current.SetSelectedGameObject(Characters[i].gameObject);
                break;
            }
        }
    }

    public void ExitSelectMenu() {
        EventSystem.current.SetSelectedGameObject(Skills[currSkillIndx].gameObject);
    }

    public string GetCurrentCharacter() {
        return currChar;
    }

    // get current skill
    public string GetClickedSkill(int skill) {
        currSkillIndx = skill;
        return currSkill;
    }

    private void updateSkillList() {
        for (int i = 0; i < Skills.Length; i++) {
            Skill skill = GameManager.Instance.Party.GetCharSkillAt(currChar, i + skillIndx);
            if (skill == null) {
                Skills[i].gameObject.SetActive(false);
                continue;
            }
            Skills[i].gameObject.SetActive(true);
            Skills[i].text = skill.SkillName;
        }
    }

    private void updateDescription() {
        Skill skill = GameManager.Instance.Party.GetCharSkill(currChar, currSkill);
        if (skill != null) {
            Description.text = "Cost: " + skill.Cost + " SP\n" + skill.Description;
        }
    }
}
