using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour {
    public Transform[] EnemyPos;
    public Enemy[] Enemies;

    public List<Enemy> GetEnemies() {
        List<Enemy> enemies = new List<Enemy>();
        if (Enemies.Length > 0) {
            foreach (Enemy enemy in Enemies) {
                enemies.Add(enemy);
            }
        } else {
            List<Enemy> spawnedEnemies = EnemySpawner.Instance.SpawnEnemies(GameManager.Instance.CurrentScene);
            for (int i = 0; i < spawnedEnemies.Count; i++) {
                Enemy enemy = Instantiate(spawnedEnemies[i], EnemyPos[i].position, EnemyPos[i].rotation);
                enemy.transform.SetParent(EnemyPos[i]);
                enemies.Add(enemy);
            }
        }

        return enemies;
    }
}
