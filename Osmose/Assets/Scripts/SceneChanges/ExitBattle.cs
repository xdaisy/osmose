using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitBattle : MonoBehaviour {
    public float WaitToLoad = 1f;

    private bool shouldLoadAfterFade;

    // Update is called once per frame
    void Update()
    {
        if (shouldLoadAfterFade) {
            WaitToLoad -= Time.deltaTime;
            if (WaitToLoad <= 0f) {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(GameManager.Instance.CurrentScene);
            }
        }
    }

    public void EndBattle() {
        shouldLoadAfterFade = true;
        UIFade.Instance.FadeToBlack();
        PlayerControls.Instance.PreviousAreaName = "Battle";
        GameManager.Instance.FadingBetweenAreas = true;
    }
}
