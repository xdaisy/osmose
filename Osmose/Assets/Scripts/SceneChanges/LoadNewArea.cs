﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewArea : MonoBehaviour {

    public string sceneToLoad;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}