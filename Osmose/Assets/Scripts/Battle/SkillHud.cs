using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillHud : MonoBehaviour {
    public CanvasGroup Hud;

    [Header("Skill UI")]
    public Text[] Skills;

    [Header("Description")]
    public Text Description;

    private string currChar; // character whose turn it is

    private string currSkill; // current highlighted skill
    private int skillIndx; // for scrolling through skills
    
    private int clickedSkill; // index of clicked button

    // Start is called before the first frame update
    void Start()
    {
        currSkill = "";
        skillIndx = 0;
        clickedSkill = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Hud.interactable) {
            Text highlightedSkill = EventSystem.current.currentSelectedGameObject.GetComponent<Text>();
            if (Input.GetButtonDown("Vertical")) {
                float buttonInput = Input.GetAxisRaw("Vertical");
                if (buttonInput < -0.5f && highlightedSkill.text == currSkill) {
                    // scroll down
                    Skill skill = GameManager.Instance.Party.GetCharSkillAt(currChar, skillIndx + Skills.Length);
                    if (skill != null) {
                        skillIndx++;
                        updateSkills();
                    }
                }
                if (buttonInput > 0.5f && highlightedSkill.text == currSkill && skillIndx > 0) {
                    // scroll up
                    skillIndx--;
                    updateSkills();
                }
            }

            currSkill = highlightedSkill.text;
            updateDescription();
        }
    }

    // open skill hud
    public void OpenSkillsHud(string charName) {
        currChar = charName;
        updateSkills();
        EventSystem.current.SetSelectedGameObject(Skills[0].gameObject);
    }

    // exit skill hud
    // reset some fields back to default
    public void ExitSkillHud() {
        currChar = "";
        currSkill = "";
        skillIndx = 0;
        Description.text = "";
    }

    // get the clicked skill's name
    public Skill GetClickedSkill(int skill) {
        clickedSkill = skill;
        return GameManager.Instance.Party.GetCharSkill(currChar, Skills[skill].text);
    }

    // set the last clicked skill as selected
    public void SetLastClickedSkill() {
        EventSystem.current.SetSelectedGameObject(Skills[clickedSkill].gameObject);
        clickedSkill = -1;
    }

    private void updateSkills() {
        for (int i = 0; i < Skills.Length; i++) {
            Button skillButton = Skills[i].GetComponent<Button>();
            Skill skill = GameManager.Instance.Party.GetCharSkillAt(currChar, i + skillIndx);
            if (skill == null) {
                skillButton.interactable = false;
                Skills[i].text = "";
                continue;
            }
            skillButton.interactable = true;
            Skills[i].text = skill.SkillName;
        }
    }

    private void updateDescription() {
        Skill skill = GameManager.Instance.Party.GetCharSkill(currChar, currSkill);
        int currSP = GameManager.Instance.Party.GetCharacterCurrentSP(currChar);
        int maxSP = GameManager.Instance.Party.GetCharacterMaxSP(currChar);

        Description.text = "SP: " + currSP + "/" + maxSP + "\n" + skill.Description;
    }
}
