using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour {

    public string cutsceneTxtPath;

    private FileInfo sourceFile;

    private StreamReader reader;

    public Text dText;
    public Text dName;

	// Use this for initialization
	void Start () {
        sourceFile = new FileInfo(cutsceneTxtPath); // get file
        reader = sourceFile.OpenText(); // open the file to read

        // get name of person saying first line of dialogue in cutscene
        string line = reader.ReadLine();
        string[] person = line.Split('-');
        dName.text = person[0];

        // get first line of dialogue in cutscene
        line = reader.ReadLine();
        dText.text = line;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Interact")) {

            if(reader.EndOfStream) {
                // if reader is at the end of the file, don't do anything
                return;
            }

            string line = reader.ReadLine();
            while(line.Length == 0) {
                // don't show empty strings
                line = reader.ReadLine();
            }
            if (line.Contains(":")) {
                // if line contains :, then is name
                string name = "";
                if (line.Contains("-")) {
                    // if have -, person talking have a specific sprite associating with text
                    string[] person = line.Split('-');
                    name = person[0];
                } else {
                    // if no -, then line is name of person talking
                    name = line;
                }

                dName.text = name; // name of person talking is always first word
                line = reader.ReadLine(); // name of person talking always followed by lines of text
            }

            dText.text = line;
        }
	}
}
