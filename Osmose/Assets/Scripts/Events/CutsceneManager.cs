using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class CutsceneManager : MonoBehaviour {

    public string cutsceneTxtPath;

    private FileInfo sourceFile;

    private StreamReader reader;

    public Text dText;
    public Text dName;
    public Image talkingSprite;

    public string sceneToLoad;

    public Image fadeScreen;
    public Animator fadeAnim;

    public string cutsceneName;

    private CutsceneSpriteHolder spriteHolder;

	// Use this for initialization
	void Start () {
        spriteHolder = GetComponent<CutsceneSpriteHolder>();
        sourceFile = new FileInfo(cutsceneTxtPath); // get file
        reader = sourceFile.OpenText(); // open the file to read

        ChangeText(); // show the first line of dialogue
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Interact")) {

            if(reader.EndOfStream) {
                // if reader is at the end of the file, don't do anything
                string[] path = cutsceneTxtPath.Split('/');
                string[] fileName = path[path.Length - 1].Split('.');
                string eventName = fileName[0]; // get name of cutscene
                CutSceneHandler.addEvent(eventName); // add the event to cutscene handler
                StartCoroutine(Fade()); // fade to next scene
                return; // don't change the text bc at end of file
            }

            ChangeText();
        }
	}

    private void ChangeText() {
        string line = reader.ReadLine();
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

    // changes the portrait of the person talking
    private string ChangeSprite(string line) {
        string[] person = line.Split('-'); // split name from expression
        string name = person[0];

        talkingSprite.enabled = true; // want to show sprite
        string spriteName = person[0] + "_" + person[1];
        Sprite personSprite = spriteHolder.GetSprite(spriteName);
        talkingSprite.sprite = personSprite;
        

        return name;
    }

    IEnumerator Fade() {
        PlayerControls.Instance.SetCanMove(false);
        fadeAnim.SetBool("Fade", true);
        yield return new WaitUntil(() => fadeScreen.color.a == 1); // wait until alpha value is one
        PlayerControls.Instance.PreviousAreaName = cutsceneName;
        SceneManager.LoadScene(sceneToLoad);
        PlayerControls.Instance.SetCanMove(true);
    }
}
