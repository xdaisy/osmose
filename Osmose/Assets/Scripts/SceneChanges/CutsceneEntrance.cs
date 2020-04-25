using UnityEngine;

public class CutsceneEntrance : MonoBehaviour {
    public string transitionFromCutscene;

    public bool ShowDialogue;
    public string[] PostDialogue;

    // Use this for initialization
    void Start() {
        if (transitionFromCutscene.Equals(GameManager.Instance.PreviousScene)) {
            // set player to entrance's position
            PlayerControls.Instance.SetPosition(transform.position);

            if (ShowDialogue) {
                Dialogue.Instance.ShowDialogue(PostDialogue, true);
            }
        }
    }
}
