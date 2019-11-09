using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that fades the screen to black and back
/// </summary>
public class UIFade : MonoBehaviour
{
    public static UIFade Instance;

    public Image FadeImage;

    private bool shouldFadeToBlack;
    private bool shouldFadeFromBlack;

    public float FadeSpeed = 1f;

    // Start is called before the first frame update
    void Start() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update() {
        if (shouldFadeToBlack) {
            FadeImage.color = new Color(
                FadeImage.color.r, 
                FadeImage.color.g, 
                FadeImage.color.b, 
                Mathf.MoveTowards(FadeImage.color.a, 1f, FadeSpeed * Time.deltaTime)
            );

            if (FadeImage.color.a == 1f) {
                shouldFadeToBlack = false;
            }
        }
        if (shouldFadeFromBlack) {
            FadeImage.color = new Color(
                FadeImage.color.r, 
                FadeImage.color.g, 
                FadeImage.color.b, 
                Mathf.MoveTowards(FadeImage.color.a, 0f, FadeSpeed * Time.deltaTime)
            );

            if (FadeImage.color.a == 0f) {
                shouldFadeFromBlack = false;
            }
        }
    }

    /// <summary>
    /// Trigger to fade to black
    /// </summary>
    public void FadeToBlack() {
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;
    }

    /// <summary>
    /// Trigger to fade from black
    /// </summary>
    public void FadeFromBlack() {
        shouldFadeFromBlack = true;
        shouldFadeToBlack = false;
        if (FadeImage.color.a != 1f) {
            // if the fade image isn't full opacity, set it to full opacity
            FadeImage.color = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, 1f);
        }
    }
}
