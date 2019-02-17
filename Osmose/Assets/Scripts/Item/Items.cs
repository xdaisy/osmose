using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    [Header("Item Type")]
    public bool IsItem;
    public bool IsWeapon;
    public bool IsArmor;

    [Header("Generic Details")]
    // used for in game stuff
    public string ItemName;
    public string Description;
    public int Value;
    public Sprite ItemSprite;

    [Header("Item Details")]
    public int AmountToChange; // for healing or boosting stats
    public bool AffectHP, AffectSP, AffectAttk, AffectDefn;
    public bool UseOnParty; // true for use on party member, false if use on enemy

    [Header("Weapon/Armor Details")]
    public int WeaponStr; // attack power for weapon

    public int ArmorDefn; // defense for armor

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Use the item
    /// </summary>
    /// <param name="charName">Name of the character to use the item on</param>
    public void Use(string charName) {
        if (IsItem && UseOnParty) {
            // is a regular item and use item
            // item affects party member

            if (AffectHP) {
                GameManager.Instance.Party.RecoverHP(charName, AmountToChange);
            }
            if (AffectSP) {
                GameManager.Instance.Party.RecoverSP(charName, AmountToChange);
            }
            if(AffectAttk) {
                GameManager.Instance.Party.IncreaseStat(charName, StatType.ATTACK, AmountToChange);
            }
            if(AffectDefn) {
                GameManager.Instance.Party.IncreaseStat(charName, StatType.DEFENSE, AmountToChange);
            }
            GameManager.Instance.RemoveItem(ItemName, 1);
        }

        if (IsWeapon) {
            if (GameManager.Instance.Party.GetWeapon(charName) != "") {
                // add weapon back to equipment
                GameManager.Instance.AddEquipment(GameManager.Instance.Party.GetWeapon(charName), 1);
            }

            GameManager.Instance.Party.EquipWeapon(charName, ItemName, WeaponStr);
            GameManager.Instance.RemoveEquipment(ItemName, 1);
        }

        if (IsArmor) {
            if (GameManager.Instance.Party.GetArmor(charName) != "") {
                // add weapon back to equipment
                GameManager.Instance.AddEquipment(GameManager.Instance.Party.GetArmor(charName), 1);
            }

            GameManager.Instance.Party.EquipArmor(charName, ItemName, ArmorDefn);
            GameManager.Instance.RemoveEquipment(ItemName, 1);
        }

    }
}
