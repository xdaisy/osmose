using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopActivator : MonoBehaviour {
    public Shop TownShop;
    public float ShopReprise = 0.1f;
    private float timeElapsed = 1f;
    private bool canOpenShop = false;

    // Update is called once per frame
    void Update()
    {
        if (timeElapsed < ShopReprise) {
            timeElapsed += Time.deltaTime;
        } else {
            if (canOpenShop && GameManager.Instance.CanOpenShop() && Input.GetButtonDown("Interact") && !TownShop.gameObject.activeSelf) {
                TownShop.OpenShopMenu();
            }
        }
    }

    // make it so that the shop menu can close without activating it again
    public void CloseShop() {
        timeElapsed = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            canOpenShop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            canOpenShop = false;
        }
    }
}
