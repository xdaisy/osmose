using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleSystem : MonoBehaviour {

    public EventSystem eventSystem;
    public CanvasGroup MainHud;
    public CanvasGroup PartyHud;
    public CanvasGroup EnemyHud;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    /// <summary>
    /// Go from the Main Battle HUD to the Select Enemy HUD
    /// </summary>
    public void SelectAttack() {
        MainHud.interactable = false;
        EnemyHud.interactable = true;

        // TODO: Find way to get list of the Enemies and have the far left one selected 
        Button enemy = EnemyHud.GetComponentInChildren<Button>();
        eventSystem.SetSelectedGameObject(enemy.gameObject);
    }
}
