﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour {

    public string loadArea;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {

    }

    // load the game
    public void StartGame() {
        SceneManager.LoadScene(loadArea, LoadSceneMode.Single);
    }
}