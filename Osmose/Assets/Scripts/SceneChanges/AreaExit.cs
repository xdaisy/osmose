using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AreaExit : MonoBehaviour {

    public string sceneToLoad;

    public Image fadeScreen;
    public Animator fadeAnim;

    public string areaName; // area's name

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade() {
        PlayerControls.instance.SetCanMove(false);
        fadeAnim.SetBool("Fade", true);
        yield return new WaitUntil(() => fadeScreen.color.a == 1); // wait until alpha value is one
        PlayerControls.instance.previousAreaName = areaName;
        SceneManager.LoadScene(sceneToLoad);
        PlayerControls.instance.SetCanMove(true);
    }
}
