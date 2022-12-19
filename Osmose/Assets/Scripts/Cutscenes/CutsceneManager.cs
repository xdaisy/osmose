using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Class that handles how the cutscenes are played out
/// </summary>
public class CutsceneManager : MonoBehaviour {
    [Header("Cutscene Info")]
    public string CutsceneName;
    public TextAsset TextFile;

    [Header("UI")]
    public Text dText;
    public Text dName;
    public Image talkingSprite;
    public Animator spriteAnim;
    public Image cgImage;
    public float FadeSpeed = 1f;

    [Header("Scene Load")]
    public SceneName sceneToLoad;
    public string RegionUnlock;
    public SceneName[] ScenesToUnlock;

    private StringReader reader;
    private CutsceneSpriteHolder spriteHolder;
    private bool IsFadingIn = false;
    private bool IsFadingOut = false;

	// Use this for initialization
	void Start () {
        spriteHolder = GetComponent<CutsceneSpriteHolder>();
        reader = new StringReader(TextFile.text); // open the file to read

        ChangeText(reader.ReadLine()); // show the first line of dialogue

        GameManager.Instance.InCutscene = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (IsFadingIn) {
            // fade cg in
            cgImage.color = new Color(
                cgImage.color.r,
                cgImage.color.g,
                cgImage.color.b,
                Mathf.MoveTowards(cgImage.color.a, 1f, FadeSpeed * Time.deltaTime)
            );

            if (cgImage.color.a == 1.0f) {
                IsFadingIn = false;
            }
            return;
        }
        if (IsFadingOut) {
            // fade cg out
            cgImage.color = new Color(
                cgImage.color.r,
                cgImage.color.g,
                cgImage.color.b,
                Mathf.MoveTowards(cgImage.color.a, 0f, FadeSpeed * Time.deltaTime)
            );

            if (cgImage.color.a == 0f) {
                IsFadingOut = false;
            }
            return;
        }

		if (Input.GetButtonDown("Interact")) {
            string line = reader.ReadLine();
            if (line == null) {
                // if reader is at the end of the file, don't do anything
                EventManager.Instance.AddEvent(CutsceneName); // add the event to cutscene handler
                changeScene(); // fade to next scene
                return; // don't change the text bc at end of file
            }

            ChangeText(line);
        }

        if (Input.GetButtonDown("Skip")) {
            EventManager.Instance.AddEvent(CutsceneName); // add the event to cutscene handler
            changeScene(); // fade to next scene
            return; // don't change the text bc at end of file
        }
	}

    /// <summary>
    /// Change the text to the line passed in
    /// </summary>
    /// <param name="line">Line that the text is being changed to</param>
    private void ChangeText(string line) {
        while (line.Length == 0) {
            // don't show empty strings
            line = reader.ReadLine();
        }
        if (line.Contains(":")) {
            // if line contains :, then is name
            line = line.Remove(line.Length - 1); // remove :
            string name = line;
            Portrait portrait = Parser.ParsePortrait(line);
            if (portrait.spriteName.Equals("Portraitless")) {
                // do NOT want to show sprite
                talkingSprite.enabled = false;
            } else {
                // change sprite
                ChangeSprite(portrait);
            }

            dName.text = portrait.name; // name of person talking is always first word
            if (portrait.animationTime > 0f) {
                // if there is an animation, play it
                StartCoroutine(playAnimationCo(portrait.animationName, portrait.animationTime));
            }
            if (portrait.showCG) {
                IsFadingIn = true;
                IsFadingOut = false;
            }
            if (portrait.hideCG) {
                IsFadingOut = true;
                IsFadingIn = false;
            }

            if (portrait.newTrack > -1) {
                SoundManager.Instance.PlayBGM(portrait.newTrack);
            }

            line = reader.ReadLine(); // name of person talking always followed by lines of text
        }

        line = Parser.PlaceNewLine(line);

        dText.text = line;
    }

    /// <summary>
    /// Change the portrait of the person talking
    /// </summary>
    /// <param name="portrait">Portrait struct that contains name and sprite name</param>
    private void ChangeSprite(Portrait portrait) {
        if (portrait.spriteName.Length == 0) {
            // if the sprite name is an empty string
            return;
        }
        talkingSprite.enabled = true; // want to show sprite
        Sprite personSprite = spriteHolder.GetSprite(portrait.spriteName);
        talkingSprite.sprite = personSprite;
    }

    /// <summary>
    /// Change the scene
    /// </summary>
    private void changeScene() {
        GameManager.Instance.PreviousScene = CutsceneName;
        PlayerControls.Instance.SetPlayerForward();
        GameManager.Instance.InCutscene = false;
        if (ScenesToUnlock.Length > 0) {
            // if there's a scene to be unlocked after this cutscene, unlock it
            foreach (SceneName scene in ScenesToUnlock) {
                EventManager.Instance.AddEvent(scene.GetSceneName());
            }
        }
        LoadSceneLogic.Instance.LoadScene(sceneToLoad.GetSceneName());
    }

    /// <summary>
    /// Coroutine for playing animation
    /// </summary>
    /// <param name="animationName">Name of the animation</param>
    /// <param name="animationTime">Time to play animation</param>
    /// <returns></returns>
    private IEnumerator playAnimationCo(string animationName, float animationTime) {
        spriteAnim.SetBool(animationName, true);
        yield return new WaitForSeconds(animationTime);
        spriteAnim.SetBool(animationName, false);
    }
}
