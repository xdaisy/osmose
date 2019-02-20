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
                name.text = "";
                Characters[i].interactable = false;
            } else {
                name.text = currentParty[i];
                Characters[i].interactable = true;
            }
        }
        EventSystem.current.SetSelectedGameObject(Characters[0].gameObject);
        currCharacter = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        updateStats();
    }

    private void updateStats() {
        Name.text = currCharacter;
        updateImage();
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

        string eqpWeapon = "" + GameManager.Instance.Party.GetWeapon(currCharacter);
        Weapon.text = eqpWeapon != "" ? eqpWeapon : "<No Weapon>";
        string eqpArmor = "" + GameManager.Instance.Party.GetArmor(currCharacter);
        Armor.text = eqpArmor != "" ? eqpArmor : "<No Armor>";
    }

    private void updateImage() {
        switch (currCharacter) {
            case "Aren":
                CharacterImage.sprite = GameManager.Instance.ArenSprite;
                break;
            case "Rey":
                CharacterImage.sprite = GameManager.Instance.ReySprite;
                break;
            case "Naoshe":
                CharacterImage.sprite = GameManager.Instance.NaosheSprite;
                break;
        }
    }
}
