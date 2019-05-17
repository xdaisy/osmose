﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SaveMenu : MonoBehaviour {
    [Header("General")]
    public Button[] SaveFiles;
    public Text[] NoData;
    public GameObject[] Data;

    [Header("Save Data UI")]
    public Text[] PlayTime;
    public Text[] Location;
    public Image[] PartyMember1;
    public Image[] PartyMember2;
    public Image[] PartyMember3;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    /// <summary>
    /// Open the save menu
    /// </summary>
    public void OpenSaveMenu() {
        UpdateSaveMenu();
        EventSystem.current.SetSelectedGameObject(SaveFiles[0].gameObject);
    }

    /// <summary>
    /// Update the save menu
    /// </summary>
    public void UpdateSaveMenu() {
        for (int i = 0; i < SaveFiles.Length; i++) {
            SaveMenuData saveData = SaveFileManager.GetSaveData(i);
            if (!saveData.Exists) {
                // save doesn't exist
                Data[i].SetActive(false); // don't show data
                NoData[i].gameObject.SetActive(true); // show no data text
                continue;
            }
            PlayTime[i].text = parsePlayTime(saveData.PlayTime);
            Location[i].text = saveData.Location;
            for (int j = saveData.CurrentParty.Count; j < 3; j++) {
                // set the images of the party members that aren't in the party inactive
                if (j == 1) {
                    PartyMember2[i].gameObject.SetActive(false);
                }
                if (j == 2) {
                    PartyMember3[i].gameObject.SetActive(false);
                }
            }
            PartyMember1[i].sprite = GameManager.Instance.GetCharSprite(saveData.CurrentParty[0]); // always at least 1 character in party
            if (1 < saveData.CurrentParty.Count) {
                // put in second character
                PartyMember2[i].sprite = GameManager.Instance.GetCharSprite(saveData.CurrentParty[1]);
            }
            if (2 < saveData.CurrentParty.Count) {
                // put in third character
                PartyMember3[i].sprite = GameManager.Instance.GetCharSprite(saveData.CurrentParty[2]);
            }
        }
    }

    /// <summary>
    /// Return the play time in hh:mm:ss format
    /// </summary>
    /// <param name="playTime">Play time</param>
    /// <returns>Playtime in a string format</returns>
    private string parsePlayTime(float playTime) {
        return "" + playTime;
    }
}
