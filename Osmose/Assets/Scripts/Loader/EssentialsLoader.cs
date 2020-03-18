﻿using UnityEngine;

public class EssentialsLoader : MonoBehaviour {
    public GameObject PlayerLoader;
    public GameObject GameMang;
    public GameObject StatsMang;
    public GameObject UICanvas;
    public GameObject EnemySpawn;
    public GameObject LoadEntrance;
    public GameObject SceneLoader;

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

        if (Menu.Instance == null) {
            Instantiate(UICanvas);
        }

        if (LoadSceneLogic.Instance == null) {
            Instantiate(SceneLoader);
        }

        Instantiate(LoadEntrance);
    }
}
