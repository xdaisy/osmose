﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadNewArea : MonoBehaviour {

    public string sceneToLoad;

    public Image fadeScreen;
    public Animator fadeAnim;

    public Vector2 newPlayerPos;

    public bool isBattleMap;

    public string transitionName;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            //SceneManager.LoadScene(sceneToLoad);
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade() {
        fadeAnim.SetBool("Fade", true);
        yield return new WaitUntil(() => fadeScreen.color.a == 1); // wait until alpha value is one
        PlayerControls.instance.transform.Translate(new Vector3(newPlayerPos.x - PlayerControls.instance.transform.position.x, newPlayerPos.y - PlayerControls.instance.transform.position.y, 0f));
        PlayerControls.instance.GetComponent<PlayerControls>().IsBattleMap = this.isBattleMap;
        SceneManager.LoadScene(sceneToLoad);
    }
}
