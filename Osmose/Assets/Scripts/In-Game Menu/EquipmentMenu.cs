using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentMenu : MonoBehaviour
{
    [Header("Characters Panel")]
    public Button[] Characters;

    [Header("Equipped Panel")]
    public Text Name;
    public Text EquippedWeapon;
    public Text EquippedArmor;

    [Header("Equipment Panel")]
    public CanvasGroup EquipmentPanel;
    public Text[] Equipments;

    private int equipmentIndx = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateEquipments() {
        for (int i = 0; i < Equipments.Length; i++) {
            Text equipmentText = Equipments[i];
            Items item = GameManager.Instance.GetEquipmentAt(i + equipmentIndx);
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
        EventSystem.current.SetSelectedGameObject(Characters[0].gameObject);
    }

    public void ShowCharacterEquipment(int character) {
        string partyMember = Characters[character].GetComponentInChildren<Text>().text;
        string eqpWeapon = GameManager.Instance.Party.GetWeapon(partyMember);
        string eqpArmor = GameManager.Instance.Party.GetArmor(partyMember);

        Name.text = partyMember;
        EquippedWeapon.text = eqpWeapon != "" ? eqpWeapon : "<No Weapon>";
        EquippedArmor.text = eqpArmor != "" ? eqpArmor : "<No Armor>";
    }
}
