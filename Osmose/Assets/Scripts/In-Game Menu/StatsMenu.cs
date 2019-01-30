using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatsMenu : MonoBehaviour
{
    private EventSystem eventSystem;
    
    public Button[] Characters;

    [Header("Character Info")]
    public Image CharacterImage;
    public Text Name;
    public Text Level;

    [Header("Status")]
    public Text HP;
    public Text SP;
    public Text TotalEXP;
    public Text EXPToNextLevel;
    public Slider EXPSlider;

    [Header("Stats")]
    public Text Attack;
    public Text Defense;
    public Text MagicDefense;
    public Text Speed;
    public Text Luck;

    [Header("Equipment")]
    public Text Weapon;
    public Text Armor;

    private string currCharacter;

    private void Awake() {
        eventSystem = EventSystem.current;
        currCharacter = "";
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        string currHighlightedChar = eventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        if (currHighlightedChar != currCharacter) {
            currCharacter = currHighlightedChar;
            updateStats();
        }
    }

    public void OpenStatsMenu() {
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
        currCharacter = eventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        updateStats();
    }

    private void updateStats() {
        Name.text = currCharacter;
        Level.text = "" + GameManager.Instance.Party.GetCharacterLevel(currCharacter);

        HP.text = "" + GameManager.Instance.Party.GetCharacterCurrentHP(currCharacter) + "/" + GameManager.Instance.Party.GetCharacterMaxHP(currCharacter);
        SP.text = "" + GameManager.Instance.Party.GetCharacterCurrentSP(currCharacter) + "/" + GameManager.Instance.Party.GetCharacterCurrentSP(currCharacter);

        int currExp = GameManager.Instance.Party.GetCharacterCurrentEXP(currCharacter);
        int expToNextLvl = GameManager.Instance.Party.GetCharacterEXPtoNextLvl(currCharacter);

        TotalEXP.text = "" + currExp;
        EXPSlider.value = ((float)currExp) / expToNextLvl;
        EXPToNextLevel.text = "" + (expToNextLvl - currExp);

        Attack.text = "" + GameManager.Instance.Party.GetCharacterAttack(currCharacter);
        Defense.text = "" + GameManager.Instance.Party.GetCharacterDefense(currCharacter);
        MagicDefense.text = "" + GameManager.Instance.Party.GetCharacterMagicDefense(currCharacter);
        Speed.text = "" + GameManager.Instance.Party.GetCharacterSpeed(currCharacter);
        Luck.text = "" + GameManager.Instance.Party.GetCharacterLuck(currCharacter);

        Weapon.text = "" + GameManager.Instance.Party.GetWeapon(currCharacter);
        Armor.text = "" + GameManager.Instance.Party.GetArmor(currCharacter);
    }
}
