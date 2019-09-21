using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that handles when a cutscene gets triggered
/// </summary>
public class CutsceneTrigger : MonoBehaviour
{
    public string CutsceneName;
    public string SceneToLoad;

    public bool HaveDialogue;
    public string[] PreDialogue;

    public float WaitToLoad = Constants.WAIT_TIME;

    private bool showingDialogue;
    private bool shouldLoadAfterFade;
    // Start is called before the first frame update
    void Start()
    {
        if (EventManager.Instance.DidEventHappened(CutsceneName)) {
            // if cutscene already happen
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update() {
        if (showingDialogue && !shouldLoadAfterFade) {
            if (!Dialogue.Instance.GetDialogueActive()) {
                // after triggering dialogue, if dialogue isn't showing anymore, load
                shouldLoadAfterFade = true;
                UIFade.Instance.FadeToBlack();
                GameManager.Instance.FadingBetweenAreas = true;
                GameManager.Instance.CurrentScene = SceneToLoad;
            }
        }
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
            if (HaveDialogue) {
                Dialogue.Instance.ShowDialogue(PreDialogue, true);
                showingDialogue = true;
            } else {
                shouldLoadAfterFade = true;
                UIFade.Instance.FadeToBlack();
                GameManager.Instance.FadingBetweenAreas = true;
                GameManager.Instance.CurrentScene = SceneToLoad;
            }
        }
    }
}
