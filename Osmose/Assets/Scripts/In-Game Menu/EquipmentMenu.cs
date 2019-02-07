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

    private string currCharacter;

    private void Awake() {
        eventSystem = EventSystem.current;
        currCharacter = "";
        currEquipment = "";
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
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                // scroll up
                if (currEquipment == EventSystem.current.currentSelectedGameObject.GetComponent<Text>().text && equipmentIndx > 1) {
                    equipmentIndx--;
                    updateEquipments();
                }
            } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                // scroll down
                if (currEquipment == EventSystem.current.currentSelectedGameObject.GetComponent<Text>().text) {
                    Items item = GameManager.Instance.GetNthEquipment(equipmentIndx + Equipments.Length, equipWeapon);
                    if (item != null) {
                        equipmentIndx++;
                        updateEquipments();
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

    private void updateEquipments() {
        for (int i = 0; i < Equipments.Length; i++) {
            Text equipmentText = Equipments[i];
            Button equipmentButton = equipmentText.GetComponent<Button>();
            Items item = GameManager.Instance.GetNthEquipment(i + equipmentIndx, equipWeapon);
            if (item == null) {
                equipmentText.text = "";
                equipmentButton.interactable = false;
                continue;
            }
            equipmentText.text = item.ItemName;
            equipmentButton.interactable = true;
        }
    }

    private void updateDescription() {
        Items equipment = GameManager.Instance.GetEquipmentDetails(currEquipment);

        StatText.text = equipment.IsWeapon ? "Attack:" : "Defense:"; // say whether or not it has attack or defense
        StatAmount.text = equipment.IsWeapon ? "" + equipment.WeaponStr : "" + equipment.ArmorDefn;
        Description.text = equipment.Description;
    }

    public void OpenEquipmentMenu() {
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

        equipWeapon = true;

        Button firstChar = Characters[0];
        EventSystem.current.SetSelectedGameObject(firstChar.gameObject);
        currCharacter = firstChar.GetComponentInChildren<Text>().text;
    }

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

    public void ShowEquipments(bool equipWeapon) {
        this.equipWeapon = equipWeapon;
        updateEquipments();
        Text equipment = Equipments[0];
        currEquipment = equipment.text;
        EventSystem.current.SetSelectedGameObject(equipment.gameObject);
        updateDescription();
    }

    // dehighlight currenty highlighted equipment
    public void ExitEquipments() {
        Button currHighlightedEqmt = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        EventSystem.current.SetSelectedGameObject(null);
        currHighlightedEqmt.interactable = false;
    }
}
