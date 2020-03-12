using UnityEngine;

public class CutsceneEntrance : MonoBehaviour {
    public string transitionFromCutscene;
    public bool IsBattleMap;

    public bool ShowDialogue;
    public string[] PostDialogue;

    // Use this for initialization
    void Start() {
        if (transitionFromCutscene.Equals(PlayerControls.Instance.PreviousAreaName)) {
            // set player to entrance's position
            PlayerControls.Instance.SetPosition(transform.position);
            GameManager.Instance.IsBattleMap = IsBattleMap;
            GameManager.Instance.FadingBetweenAreas = false;
        }
        UIFade.Instance.FadeFromBlack();
        if (ShowDialogue) {
            Dialogue.Instance.ShowDialogue(PostDialogue, true);
        }
    }
}
