using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AreaExit : MonoBehaviour {

    public string SceneToLoad;

    public Image FadeScreen;
    public Animator FadeAnim;

    public string AreaName; // area's name

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
        PlayerControls.Instance.SetCanMove(false);
        FadeAnim.SetBool("Fade", true);
        yield return new WaitUntil(() => FadeScreen.color.a == 1); // wait until alpha value is one
        PlayerControls.Instance.PreviousAreaName = AreaName;
        SceneManager.LoadScene(SceneToLoad);
        PlayerControls.Instance.SetCanMove(true);
    }
}
