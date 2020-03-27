using UnityEngine;

public class LoadEntrance : MonoBehaviour {
    private string transitionFromArea = "Continue";

    // Use this for initialization
    void Start() {
        if (transitionFromArea == GameManager.Instance.PreviousScene) {
            GameManager.Instance.FadingBetweenAreas = false;
        }
        UIFade.Instance.FadeFromBlack();
    }
}
