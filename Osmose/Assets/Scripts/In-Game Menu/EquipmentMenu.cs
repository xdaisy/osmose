using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentMenu : MonoBehaviour {
    private EventSystem eventSystem;

    [Header("Characters Panel")]
    public Button[] Characters;

    [Header("Equipped Panel")]
    public CanvasGroup EquippedPanel;
    public Text Name;
    public Text EquippedWeapon;
    public Text EquippedArmor;

    [Header("Equipment Panel")]
    public CanvasGroup EquipmentPanel;
    public Text[] Equipments;

    [Header("Description Panel")]
    public Text StatText;
    public Text StatAmount;
    public Image EquipmentImage;
    public Text Description;

    private int equipmentIndx; // always start at the first weapon/armor

    private bool equipWeapon; // true if looking for weapon, false otherwise

    private string currEquipmentButton;

    private string currEquipment;
    private int currEqmtIndx;

    private string currCharacter;

    private void Awake() {
        eventSystem = EventSystem.current;
        currCharacter = "";
        currEquipment = "";
        currEqmtIndx = 0;
        currEquipmentButton = "";
        equipmentIndx = 1;
        equipWeapon = true;
    }

    // Start is called before the first frame update
    void Start() {
        currCharacter = "";
        currEquipment = "";
        currEquipmentButton = "";
        equipmentIndx = 1;
        equipWeapon = true;
        //eventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (EquippedPanel.interactable) {
            // equipped panel is interactable
            GameObject highlightedEqpButton = EventSystem.current.currentSelectedGameObject;
            if (currEquipmentButton != highlightedEqpButton.name) {
                // if not looking at previous equipment type, change equipment type
                equipWeapon = !equipWeapon; // change equipment choice
                currEquipmentButton = highlightedEqpButton.name;
                updateEquipments();
            }
            currEquipmentButton = highlightedEqpButton.name;
        } else if (EquipmentPanel.interactable) {
            // equipment panel is interactable
            if (Input.GetButtonDown("Vertical")) {
                float buttonInput = Input.GetAxisRaw("Vertical");
                if (buttonInput > 0.5f) {
                    // scroll up
                    if (currEquipment == EventSystem.current.currentSelectedGameObject.GetComponent<Text>().text && equipmentIndx > 1) {
                        equipmentIndx--;
                        updateEquipments();
                    }
                } else if (buttonInput < 0.5f) {
                    // scroll down
                    if (currEquipment == EventSystem.current.currentSelectedGameObject.GetComponent<Text>().text) {
                        Items item = GameManager.Instance.GetNthEquipment(equipmentIndx + Equipments.Length, equipWeapon, currCharacter);
                        if (item != null) {
                            equipmentIndx++;
                            updateEquipments();
                        }
                    }
                }
            }

            string highlightedEqp = EventSystem.current.currentSelectedGameObject.GetComponent<Text>().text;
            if (currEquipment != highlightedEqp) {
                currEquipment = highlightedEqp;
                updateDescription();
            }
        } else {
            // character select panel is interactable
            string charName = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
            if (charName != currCharacter) {
                // if current character != highlighted character, update the equipped panel
                for (int i = 0; i < Characters.Length; i++) {
                    if (charName == Characters[i].GetComponentInChildren<Text>().text) {
                        ShowCharacterEquipment(i);
                        break;
                    }
                }
            }
            currCharacter = charName;
        }
    }

    /// <summary>
    /// Update the equipment shown on Equipments panel
    /// </summary>
    private void updateEquipments() {
        for (int i = 0; i < Equipments.Length; i++) {
            Text equipmentText = Equipments[i];
            Items item = GameManager.Instance.GetNthEquipment(i + equipmentIndx, equipWeapon, currCharacter);
            if (item == null) {
                equipmentText.gameObject.SetActive(false);
                continue;
            }
            equipmentText.gameObject.SetActive(true);
            equipmentText.GetComponent<Button>().interactable = true;
            equipmentText.text = item.ItemName;
        }
    }

    /// <summary>
    /// Update the description
    /// </summary>
    private void updateDescription() {
        GameObject currentEquipment = EventSystem.current.currentSelectedGameObject;
        Items equipment = GameManager.Instance.GetEquipmentDetails(currEquipment);
        if (currentEquipment.activeSelf && equipment != null) {
            StatText.text = equipment.IsWeapon ? "Attack:" : "Defense:"; // say whether or not it has attack or defense
            StatAmount.text = equipment.IsWeapon ? "" + equipment.WeaponStr : "" + equipment.ArmorDefn;
            Description.text = equipment.Description;

            if (equipment.ItemSprite != null) {
                EquipmentImage.gameObject.SetActive(true);
                EquipmentImage.sprite = equipment.ItemSprite;
            }
        } else {
            StatText.text = "";
            StatAmount.text = "";
            Description.text = "";
            EquipmentImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Set up the Equipment Menu when opening it
    /// </summary>
    public void OpenEquipmentMenu() {
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

        equipWeapon = true;

        Button firstChar = Characters[0];
        EventSystem.current.SetSelectedGameObject(firstChar.gameObject);
        currCharacter = firstChar.GetComponentInChildren<Text>().text;

        updateDescription();
    }

    /// <summary>
    /// Setup the Equipments Panel when selected a character to equip
    /// </summary>
    /// <param name="character">Name of the character who's being equipped</param>
    public void ShowCharacterEquipment(int character) {
        string partyMember = Characters[character].GetComponentInChildren<Text>().text;
        string eqpWeapon = GameManager.Instance.Party.GetWeapon(partyMember);
        string eqpArmor = GameManager.Instance.Party.GetArmor(partyMember);

        Name.text = partyMember;
        EquippedWeapon.text = eqpWeapon != "" ? eqpWeapon : "<No Weapon>";
        EquippedArmor.text = eqpArmor != "" ? eqpArmor : "<No Armor>";

        currEquipmentButton = EventSystem.current.currentSelectedGameObject.name;

        equipWeapon = true;
        updateEquipments();
    }

    /// <summary>
    /// Get the name of the current character
    /// </summary>
    /// <returns>Name of the current character</returns>
    public string GetCurrentCharacter() {
        return currCharacter;
    }

    /// <summary>
    /// Get name of the equipment currently clicked on
    /// </summary>
    /// <param name="indx">Index of the equipment</param>
    /// <returns>Name of the equipment</returns>
    public string GetEquipment(int indx) {
        currEqmtIndx = indx;
        return Equipments[indx].text;
    }

    /// <summary>
    /// Update the equipment panel after equipping
    /// </summary>
    public void UpdateAfterEquip() {
        string eqpWeapon = GameManager.Instance.Party.GetWeapon(currCharacter);
        string eqpArmor = GameManager.Instance.Party.GetArmor(currCharacter);

        EquippedWeapon.text = eqpWeapon != "" ? eqpWeapon : "<No Weapon>";
        EquippedArmor.text = eqpArmor != "" ? eqpArmor : "<No Armor>";

        updateEquipments();
        currEqmtIndx--;
        if (currEqmtIndx < 0) {
            currEqmtIndx = 0;
        }
        Text newCurrEqmt = Equipments[currEqmtIndx];
        currEquipment = newCurrEqmt.text;
        eventSystem.SetSelectedGameObject(newCurrEqmt.gameObject);
    }

    /// <summary>
    /// Show either weapons or armor in Equipment Panel
    /// </summary>
    /// <param name="equipWeapon">Flag for whether or not is equipping weapons</param>
    public void ShowEquipments(bool equipWeapon) {
        this.equipWeapon = equipWeapon;
        updateEquipments();
        Text equipment = Equipments[0];
        currEquipment = equipment.text;
        EventSystem.current.SetSelectedGameObject(equipment.gameObject);
        updateDescription();
    }

    /// <summary>
    /// Exit out of the Equipment Panel
    /// </summary>
    public void ExitEquipments() {
        Button currHighlightedEqmt = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        EventSystem.current.SetSelectedGameObject(null);
        
        currEquipment = "";
        equipmentIndx = 1;

        StatText.text = "";
        StatAmount.text = "";
        Description.text = "";
        EquipmentImage.gameObject.SetActive(false);
    }
}
