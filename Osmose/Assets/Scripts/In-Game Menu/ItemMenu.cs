using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemMenu : MonoBehaviour {
    private EventSystem eventSystem;

    public CanvasGroup ItemList;

    public Text[] Items;

    public Sprite ItemImage;
    public Text ItemAmount;
    public Text Description;

    private Text currentItem;

    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current;
        currentItem = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && ItemList.gameObject.activeSelf && ItemList.interactable) {
            // only update if item hud is active and the item list is interactable
            if (currentItem.name == eventSystem.currentSelectedGameObject.name) {
                // if was at the is on the last item, scroll down
            }
        }
    }
}
