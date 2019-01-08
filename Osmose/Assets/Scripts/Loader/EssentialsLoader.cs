using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    public GameObject UiFader;
    public GameObject PlayerLoader;
    public GameObject GameMang;
    public GameObject DialogueCanvas;

    private void Awake() {
        if (UIFade.Instance == null) {
            Instantiate(UiFader);
        }

        if (PlayerControls.Instance == null) {
            Instantiate(PlayerLoader);
        }

        if (GameManager.Instance == null) {
            Instantiate(GameMang);
        }

        if (Dialogue.Instance == null) {
            Instantiate(DialogueCanvas);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
