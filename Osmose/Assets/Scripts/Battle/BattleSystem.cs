using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleSystem : MonoBehaviour {

    public CanvasGroup MainHud;
    public CanvasGroup PartyHud;
    public CanvasGroup EnemyHud;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Attack() {
        MainHud.interactable = false;
        EnemyHud.interactable = true;
        Button enemy = EnemyHud.GetComponent<Button>();
        EventSystem enemyEventSys = EnemyHud.GetComponent<EventSystem>();
        enemyEventSys.SetSelectedGameObject(enemy.gameObject);
    }
}
