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

    private int equipmentIndx; // always start at the first weapon/armor

    private bool equipWeapon; // true if looking for weapon, false otherwise

    private string currEquipmentButton;

    private string currEquipment;

    private string currCharacter;

    private void Awake() {
        eventSystem = EventSystem.current;
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
            GameObject highlightedEqpButton = eventSystem.currentSelectedGameObject;
            if (currEquipmentButton != highlightedEqpButton.name) {
                // if not looking at previous equipment type, change equipment type
                equipWeapon = !equipWeapon; // change equipment choice
                currEquipmentButton = highlightedEqpButton.name;
                updateEquipments();
            }
        } else if (EquipmentPanel.interactable) {
            // equipment panel is interactable
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                string highlightedEqp = eventSystem.currentSelectedGameObject.GetComponent<Text>().text;
                if (currEquipment == highlightedEqp && equipmentIndx > 1) {
                    equipmentIndx--;
                    updateEquipments();
                }
            } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                string highlightedEqp = eventSystem.currentSelectedGameObject.GetComponent<Text>().text;
                if (currEquipment == highlightedEqp) {
                    Items item = GameManager.Instance.GetNthEquipment(equipmentIndx + Equipments.Length, equipWeapon);
                    if (item != null) {
                        equipmentIndx++;
                        updateEquipments();
                    }
                }
            }
        } else {
            // character select panel is interactable
            string charName = eventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text;
            if (charName != currCharacter) {
                // if current character != highlighted character, update the equipped panel
                for (int i = 0; i < Characters.Length; i++) {
                    if (charName == Characters[i].GetComponentInChildren<Text>().text) {
                        ShowCharacterEquipment(i);
                        break;
                    }
                }
            }
        }
    }

    private void updateEquipments() {
        for (int i = 0; i < Equipments.Length; i++) {
            Text equipmentText = Equipments[i];
            Items item = GameManager.Instance.GetNthEquipment(i + equipmentIndx, equipWeapon);
            if (item == null) {
                equipmentText.text = "";
                equipmentText.GetComponent<Button>().interactable = false;
                continue;
            }
            equipmentText.text = item.ItemName;
        }
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

        currEquipmentButton = eventSystem.currentSelectedGameObject.name;

        equipWeapon = true;
        updateEquipments();
    }

    public void ShowEquipments(bool equipWeapon) {
        this.equipWeapon = equipWeapon;
        updateEquipments();
        Text equipment = Equipments[0];
        currEquipment = equipment.text;
        eventSystem.SetSelectedGameObject(equipment.gameObject);
    }
}
