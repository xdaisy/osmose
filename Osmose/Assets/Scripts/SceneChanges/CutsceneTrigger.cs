using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneTrigger : MonoBehaviour
{
    public string CutsceneName;
    public string SceneToLoad;

    public float WaitToLoad = 1f;

    private bool shouldLoadAfterFade;
    // Start is called before the first frame update
    void Start()
    {
        if (EventManager.DidEventHappened(CutsceneName)) {
            // if cutscene already happen
            Destroy(this.gameObject);
        }
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
            GameManager.Instance.FadingBetweenAreas = true;
            GameManager.Instance.CurrentScene = SceneToLoad;
        }
    }
}
