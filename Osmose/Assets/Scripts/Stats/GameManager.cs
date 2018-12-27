using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PartyStats Party;

    public bool GameMenuOpen, DialogActive, FadingBetweenAreas;

    public string[] ItemsHeld; // keep track of which item the player has
    public int[] NumOfItems; // keep track of how many of each item held
    public Items[] ReferenceItems; // reference to prefab of the items

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            Party = new PartyStats();
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMenuOpen || DialogActive || FadingBetweenAreas) {
            PlayerControls.Instance.SetCanMove(false);
        } else {
            PlayerControls.Instance.SetCanMove(true);
        }
    }

    public Items GetItemDetails(string itemName) {
        foreach(Items item in ReferenceItems) {
            if (item.ItemName == itemName) {
                return item;
            }
        }

        return null;
    }

    /// <summary>
    /// Move the indexes with items to the front of the array
    /// </summary>
    public void CompressItem() {
        bool itemAfterSpace = true;

        while (itemAfterSpace) {
            itemAfterSpace = false;
            for (int i = 0; i < ItemsHeld.Length - 1; i++) {
                if (ItemsHeld[i] == "") {
                    ItemsHeld[i] = ItemsHeld[i + 1];
                    ItemsHeld[i + 1] = "";

                    NumOfItems[i] = NumOfItems[i + 1];
                    NumOfItems[i + 1] = 0;

                    if (ItemsHeld[i] != "") {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }
}
