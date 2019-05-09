using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatsMenu : MonoBehaviour
{
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
        currCharacter = "";
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        string currHighlightedChar = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
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
                Characters[i].gameObject.SetActive(false);
                continue;
            }
            Characters[i].gameObject.SetActive(true);
            name.text = currentParty[i];
        }
        EventSystem.current.SetSelectedGameObject(Characters[0].gameObject);
        currCharacter = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        updateStats();
    }

    private void updateStats() {
        Name.text = currCharacter;
        updateImage();
        Level.text = "" + GameManager.Instance.Party.GetCharLvl(currCharacter);

        HP.text = "" + GameManager.Instance.Party.GetCharCurrHP(currCharacter) + "/" + GameManager.Instance.Party.GetCharMaxHP(currCharacter);
        SP.text = "" + GameManager.Instance.Party.GetCharCurrSP(currCharacter) + "/" + GameManager.Instance.Party.GetCharCurrSP(currCharacter);

        int currExp = GameManager.Instance.Party.GetCharCurrEXP(currCharacter);
        int expToNextLvl = GameManager.Instance.Party.GetCharEXPtoNextLvl(currCharacter);

        TotalEXP.text = "" + currExp;
        EXPSlider.value = ((float)currExp) / expToNextLvl;
        EXPToNextLevel.text = "" + (expToNextLvl - currExp);

        Attack.text = "" + GameManager.Instance.Party.GetCharAttk(currCharacter);
        Defense.text = "" + GameManager.Instance.Party.GetCharDef(currCharacter);
        MagicDefense.text = "" + GameManager.Instance.Party.GetCharMagDef(currCharacter);
        Speed.text = "" + GameManager.Instance.Party.GetCharSpd(currCharacter);
        Luck.text = "" + GameManager.Instance.Party.GetCharLck(currCharacter);

        string eqpWeapon = "" + GameManager.Instance.Party.GetWeapon(currCharacter);
        Weapon.text = eqpWeapon != "" ? eqpWeapon : "<No Weapon>";
        string eqpArmor = "" + GameManager.Instance.Party.GetArmor(currCharacter);
        Armor.text = eqpArmor != "" ? eqpArmor : "<No Armor>";
    }

    private void updateImage() {
        switch (currCharacter) {
            case Constants.AREN:
                CharacterImage.sprite = GameManager.Instance.ArenSprite;
                break;
            case Constants.REY:
                CharacterImage.sprite = GameManager.Instance.ReySprite;
                break;
            case Constants.NAOISE:
                CharacterImage.sprite = GameManager.Instance.NaoiseSprite;
                break;
        }
    }
}
