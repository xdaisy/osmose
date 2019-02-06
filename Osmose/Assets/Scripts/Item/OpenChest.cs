﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{
    public Items Item;
    public int Amount;

    private Animator anim;

    private bool canOpen;
    private bool opened;
    
    // Start is called before the first frame update
    void Start()
    {
        canOpen = false;
        opened = false;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen && !opened && Input.GetButtonDown("Interact")) {
            opened = true;

            if (Item.IsItem) {
                // add item
                GameManager.Instance.AddItem(Item.ItemName, Amount);
            } else if (Item.IsWeapon || Item.IsArmor) {
                // add equipment
                GameManager.Instance.AddEquipment(Item.ItemName, Amount);
            } else {
                // add key item
                GameManager.Instance.AddKeyItem(Item.ItemName);
            }

            anim.SetTrigger("OpenChest");

            string text = "You got " + Amount + " " + Item.ItemName + "!";
            string[] dialogToShow = { text };

            Dialogue.Instance.ShowDialogue(dialogToShow);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            canOpen = false;
        }
    }
}