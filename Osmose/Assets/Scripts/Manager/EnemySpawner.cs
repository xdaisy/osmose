using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AreaDictionary : SerializableDictionaryBase<string, EnemyDictionary> { }

[Serializable]
public class EnemyDictionary : SerializableDictionaryBase<int, Enemy> { }

public class EnemySpawner : MonoBehaviour {
    public AreaDictionary Enemies;

    public static EnemySpawner Instance;

    // Start is called before the first frame update
    void Start() {
        if (Instance == null) {
            Instance = this;
        } else {
            // if another enemy spawner exists, destroy game object
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
