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
        // get a random number
        System.Random rnd = new System.Random();
        int i = rnd.Next(Enemies[area].Keys.Count);
        // add the dictionary of enemies to the enemies list
        enemies.Add(Enemies[area][i]);
        return enemies;
    }
}
