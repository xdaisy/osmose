using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour {
    public GameObject PlayerLoader;
    public GameObject GameMang;
    public GameObject StatsMang;
    public GameObject DialogueCanvas;
    public GameObject FadeCanvas;
    public GameObject MenuCanvas;
    public GameObject EnemySpawn;
    public GameObject LoadEntrance;

    private void Awake() {
        if (PlayerControls.Instance == null) {
            Instantiate(PlayerLoader);
        }

        if (StatsManager.Instance == null) {
            Instantiate(StatsMang);
        }

        if (GameManager.Instance == null) {
            Instantiate(GameMang);
        }

        if (EnemySpawner.Instance == null) {
            Instantiate(EnemySpawn);
        }

        if (Dialogue.Instance == null) {
            Instantiate(DialogueCanvas);
        }

        if (UIFade.Instance == null) {
            Instantiate(FadeCanvas);
        }

        if (Menu.Instance == null) {
            Instantiate(MenuCanvas);
        }

        Instantiate(LoadEntrance);
    }
}
