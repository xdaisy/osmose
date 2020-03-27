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
            List<Enemy> spawnedEnemies = EnemySpawner.Instance.SpawnEnemies(
                GameManager.Instance.PreviousScene
            );

            Dictionary<string, int> enemyCount = new Dictionary<string, int>();
            for (int i = 0; i < spawnedEnemies.Count; i++) {
                Enemy enemy = Instantiate(spawnedEnemies[i], EnemyPos[i].position, EnemyPos[i].rotation);

                if (enemyCount.ContainsKey(enemy.EnemyName)) {
                    // if have more than 1 of same enemy, adjust name so can differentiate by appending a number
                    string enemyName = enemy.EnemyName;
                    int count = enemyCount[enemyName] + 1;
                    enemy.SetEnemyID(count);
                    enemyCount[enemyName] = count;
                } else {
                    enemyCount.Add(enemy.EnemyName, 1);
                }

                enemy.transform.SetParent(EnemyPos[i]);
                enemies.Add(enemy);
            }
        }

        return enemies;
    }
}
