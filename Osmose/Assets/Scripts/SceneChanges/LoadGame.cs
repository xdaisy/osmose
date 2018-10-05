using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour {

    public string loadArea;

    public Image fadeScreen;
    public Animator fadeAnim;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {

    }

    // load the game
    public void StartGame() {
        StartCoroutine(Fade()); // fade to next screen
    }

    IEnumerator Fade() {
        fadeAnim.SetBool("Fade", true);
        yield return new WaitUntil(() => fadeScreen.color.a == 1); // wait until alpha value is one
        SceneManager.LoadScene(loadArea);
    }


}
