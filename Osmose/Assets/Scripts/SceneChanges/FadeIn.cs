using UnityEngine;

/// <summary>
/// Class that fade the scene back from black
/// </summary>
public class FadeIn : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        if (GameManager.Instance.FadingBetweenAreas) {
            // if is changing between scenes
            GameManager.Instance.FadingBetweenAreas = false;
            UIFade.Instance.FadeFromBlack();
        }
    }
}
