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

    [Header("Weapon/Armor Details")]
    public int WeaponStr; // attack power for weapon

    public int ArmorStr; // defense for armor

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
