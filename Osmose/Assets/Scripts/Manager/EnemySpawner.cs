using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AreaDictionary : SerializableDictionaryBase<string, EnemyDictionary> { }

[Serializable]
public class EnemyDictionary : SerializableDictionaryBase<int, Enemy> { }

public class EnemySpawner : MonoBehaviour {
    public AreaDictionary Enemies;

    public static EnemySpawner Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            // if another player exists, destroy game object
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public List<Enemy> SpawnEnemies(string area) {
        List<Enemy> enemies = new List<Enemy>();
        enemies.Add(Enemies[area][0]);

        return enemies;
    }
}
