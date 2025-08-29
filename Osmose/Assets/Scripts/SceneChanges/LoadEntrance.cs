using UnityEngine;

public class LoadEntrance : MonoBehaviour {
    private string transitionFromArea = "Continue";

    // Use this for initialization
    void Start() {
        if (transitionFromArea == GameManager.Instance.PreviousScene) {
            InteractMark mark = GameObject.FindObjectOfType<InteractMark>();
            mark.SetMarkOff();
            GameManager.Instance.FadingBetweenAreas = false;
        }
        UIFade.Instance.FadeFromBlack();
    }
}
