using UnityEngine;

/// <summary>
/// Class that handles when a cutscene gets triggered
/// </summary>
public class CutsceneTrigger : MonoBehaviour
{
    public string CutsceneName;
    public SceneName SceneToLoad;

    public bool HaveDialogue;
    public string[] PreDialogue;

    private bool showingDialogue;
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
        if (showingDialogue) {
            if (!Dialogue.Instance.GetDialogueActive()) {
                // after triggering dialogue, if dialogue isn't showing anymore, load
                LoadSceneLogic.Instance.LoadScene(SceneToLoad.GetSceneName());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (HaveDialogue) {
                Dialogue.Instance.ShowDialogue(PreDialogue, true);
                showingDialogue = true;
            } else {
                LoadSceneLogic.Instance.LoadScene(SceneToLoad.GetSceneName());
            }
        }
    }
}
