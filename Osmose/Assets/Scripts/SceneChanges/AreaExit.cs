using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AreaExit : MonoBehaviour {

    public string SceneToLoad;

    public string AreaName; // area's name

    public float WaitToLoad = 1f; 

    private bool shouldLoadAfterFade;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (shouldLoadAfterFade) {
            WaitToLoad -= Time.deltaTime;
            if (WaitToLoad <= 0f) {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(SceneToLoad);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            shouldLoadAfterFade = true;
            UIFade.Instance.FadeToBlack();
            PlayerControls.Instance.PreviousAreaName = AreaName;
            GameManager.Instance.FadingBetweenAreas = true;
        }
    }
}
