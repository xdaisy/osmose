﻿using System.Collections;
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

    private string currChar;
    private int skillIndx;
    private string currSkill;

    // Start is called before the first frame update
    void Start() {
        currChar = "";
        skillIndx = 0;
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

    public void OpenSkillsPanel() {
        EventSystem.current.SetSelectedGameObject(Skills[0].gameObject);
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

    }
}
