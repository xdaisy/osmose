using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour {
    [Header("General")]
    public RectTransform creditsText;
    public Image cgImage;

    [SerializeField] private SceneName mainScene;
    [SerializeField] private float speed = 200.0f;
    [SerializeField] private float fadeSpeed = 1.0f;

    // Update is called once per frame
    void Update() {
        // start credits after doing fading in
        if (UIFade.Instance.IsFadeDone()) {
            RectTransform rectTransform = GetComponent<RectTransform>();
            if (creditsText.offsetMin.y >= rectTransform.sizeDelta.y) {
                if (cgImage.color.a < 1.0f) {
                    // show cg
                    cgImage.color = new Color(
                        cgImage.color.r,
                        cgImage.color.g,
                        cgImage.color.b,
                        Mathf.MoveTowards(cgImage.color.a, 1f, fadeSpeed * Time.deltaTime)
                    );
                } else if (Input.GetButtonDown("Interact")) {
                    // go to main menu if click on interact button
                    GameManager.Instance.InCutscene = false;
                    LoadSceneLogic.Instance.LoadScene(mainScene.GetSceneName());
                }
            } else {
                // scroll credits
                creditsText.transform.position = creditsText.transform.position + Vector3.up * speed * Time.deltaTime;
            }
        }
    }
}
