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

    [Header("Scene Load")]
    public string sceneToLoad;
    public float WaitToLoad = 1f;

    private StringReader reader;

    private CutsceneSpriteHolder spriteHolder;

    private bool shouldLoadAfterFade = false;

	// Use this for initialization
	void Start () {
        spriteHolder = GetComponent<CutsceneSpriteHolder>();
        reader = new StringReader(TextFile.text); // open the file to read

        UIFade.Instance.FadeFromBlack();

        ChangeText(reader.ReadLine()); // show the first line of dialogue

        GameManager.Instance.InCutscene = true;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Interact") && !shouldLoadAfterFade) {
            string line = reader.ReadLine();
            if (line == null) {
                // if reader is at the end of the file, don't do anything
                EventManager.Instance.AddEvent(CutsceneName); // add the event to cutscene handler
                changeScene(); // fade to next scene
                return; // don't change the text bc at end of file
            }

            ChangeText(line);
        }

        if (Input.GetButtonDown("Skip") && !shouldLoadAfterFade) {
            EventManager.Instance.AddEvent(CutsceneName); // add the event to cutscene handler
            changeScene(); // fade to next scene
            return; // don't change the text bc at end of file
        }

        if (shouldLoadAfterFade) {
            WaitToLoad -= Time.deltaTime;
            if (WaitToLoad <= 0f) {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(sceneToLoad);
            }
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
            if (line.Contains("-")) {
                name = ChangeSprite(line);
            } else if(name.Equals("Portraitless")) {
                // do NOT want to show sprite
                talkingSprite.enabled = false;
                name = ""; // set name to empty string
            }

            dName.text = name; // name of person talking is always first word
            line = reader.ReadLine(); // name of person talking always followed by lines of text
        }

        if (line.Contains("\\n")) {
            // be able to place new lines in the dialogue box
            line = line.Replace("\\n", Environment.NewLine); // replace \n with new line
        }

        dText.text = line;
    }

    /// <summary>
    /// Change the portrait of the person talking
    /// </summary>
    /// <param name="line">Line from the script</param>
    /// <returns>Name of the person talking</returns>
    private string ChangeSprite(string line) {
        string[] person = line.Split('-'); // split name from expression
        string name = person[0];

        talkingSprite.enabled = true; // want to show sprite
        string spriteName = person[0] + "_" + person[1];
        Sprite personSprite = spriteHolder.GetSprite(spriteName);
        talkingSprite.sprite = personSprite;
        

        return name;
    }

    /// <summary>
    /// Change the scene
    /// </summary>
    private void changeScene() {
        UIFade.Instance.FadeToBlack();
        shouldLoadAfterFade = true;
        PlayerControls.Instance.PreviousAreaName = CutsceneName;
        PlayerControls.Instance.SetPlayerForward();
        GameManager.Instance.InCutscene = false;
        GameManager.Instance.FadingBetweenAreas = true;
    }
}
