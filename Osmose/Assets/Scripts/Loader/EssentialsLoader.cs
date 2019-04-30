using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    public GameObject PlayerLoader;
    public GameObject GameMang;
    public GameObject UICanvas;
    public GameObject LoadEntrance;

    private void Awake() {
        if (PlayerControls.Instance == null) {
            Instantiate(PlayerLoader);
        }

        if (GameManager.Instance == null) {
            Instantiate(GameMang);
        }

        Instantiate(LoadEntrance);

        Instantiate(UICanvas);
    }
}
