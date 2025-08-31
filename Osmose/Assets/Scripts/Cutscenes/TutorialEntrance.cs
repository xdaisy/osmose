using System.Collections.Generic;
using UnityEngine;

public class TutorialEntrance : MonoBehaviour {
    public SceneName transitionFromCutscene;
    
    public string[] PostDialogue;

    private List<string> tutorial = new List<string> {
        "When you see an exclamation mark, it means that you can interact with the object using the SPACEBAR.",
        "Move using either the arrow keys or ASWD.",
        "Open the menu with M.",
        "Cancel with C or ESC."
    };

    // Use this for initialization
    void Start() {
        if (transitionFromCutscene.GetSceneName().Equals(GameManager.Instance.PreviousScene)) {
            // set player to entrance's position
            PlayerControls.Instance.SetPosition(transform.position);
            List<string> dialogue = new List<string>();
            
            if (!GameManager.Instance.DidSeeTutorial()) {
                // show tutorial if haven't seen the tutorial yet
                dialogue.AddRange(tutorial);
            }
            // show the dialogue
            dialogue.AddRange(PostDialogue);

            Dialogue.Instance.ShowDialogue(dialogue.ToArray(), true);
        }
    }
}
