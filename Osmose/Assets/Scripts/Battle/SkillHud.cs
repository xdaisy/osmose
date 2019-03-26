using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillHud : MonoBehaviour {
    public CanvasGroup Hud;

    [Header("Skill UI")]
    public Text[] Skills;
    public Text[] SkillCost;

    [Header("Description")]
    public Text Description;

    private string currChar; // character whose turn it is
    private bool arenShifted; // keep track if Aren is shifted

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

    // open skill hud
    public void OpenSkillsHud(string charName, bool isShifted) {
        currChar = charName;
        arenShifted = isShifted;
        updateSkills();
        for (int i = 0; i < Skills.Length; i++) {
            if (Skills[i].GetComponent<Button>().interactable) {
                EventSystem.current.SetSelectedGameObject(Skills[i].gameObject);
                break;
            }
        }
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
            Button button = Skills[i].GetComponent<Button>();
            button.interactable = true;
            Skill skill = GameManager.Instance.Party.GetCharSkillAt(currChar, i + skillIndx);
            if (skill == null) {
                Skills[i].gameObject.SetActive(false);
                continue;
            }
            Skills[i].gameObject.SetActive(true);
            Skills[i].text = skill.SkillName;
            SkillCost[i].text = "" + skill.Cost;
            if (currChar == "Aren") {
                button.interactable = arenShifted == skill.UseInShift;
            }
        }
    }

    private void updateDescription() {
        Skill skill = GameManager.Instance.Party.GetCharSkill(currChar, currSkill);
        if (skill != null) {
            Description.text = skill.Description;
        }
    }
}
