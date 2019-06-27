using System.Collections.Generic;
using UnityEngine;

public struct Stats {
    public int HP, SP, Attack, Defense, MagicDefense, Speed, Luck;
}

public struct EnemyStats {
    public int HP, SP, Attack, Defense, MagicDefense, Speed, Luck, EXP, Money;
}

public class StatsParser : MonoBehaviour {
    public TextAsset ArenStats;
    public TextAsset ReyStats;
    public TextAsset NaoiseStats;
    public TextAsset EnemyStats;

    /// <summary>
    /// Parse and return the stats for a character
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Arrau of character Stats</returns>
    public Stats[] GetProtagStats(string name) {
        Stats[] stats = new Stats[101];

        string[] charStats = null;

        switch(name) {
            case Constants.AREN:
                charStats = ArenStats.text.Split('\n');
                break;
            case Constants.REY:
                charStats = ReyStats.text.Split('\n');
                break;
            case Constants.NAOISE:
                charStats = NaoiseStats.text.Split('\n');
                break;
        }


        for (int i = 1; i < charStats.Length - 1; i++) {
            string[] levelStats = charStats[i].Split(',');

            int level = int.Parse(levelStats[0]);

            stats[level] = new Stats {
                HP = int.Parse(levelStats[1]),
                SP = int.Parse(levelStats[2]),
                Attack = int.Parse(levelStats[3]),
                Defense = int.Parse(levelStats[4]),
                MagicDefense = int.Parse(levelStats[5]),
                Speed = int.Parse(levelStats[6]),
                Luck = int.Parse(levelStats[7])
            };
        }
        return stats;
    }

    /// <summary>
    /// Parse and return the enemy stats
    /// </summary>
    /// <returns>Dictionary of the name of the enemy to its stats</returns>
    public Dictionary<string, EnemyStats> GetEnemyStats() {
        Dictionary<string, EnemyStats> enemies = new Dictionary<string, EnemyStats>();

        string[] enemyStats = EnemyStats.text.Split('\n');

        for (int i = 1; i < enemyStats.Length - 1; i++) {
            string[] stats = enemyStats[i].Split(',');

            string enemyName = stats[0];

            EnemyStats eStats = new EnemyStats {
                HP = int.Parse(stats[1]),
                SP = int.Parse(stats[2]),
                Attack = int.Parse(stats[3]),
                Defense = int.Parse(stats[4]),
                MagicDefense = int.Parse(stats[5]),
                Speed = int.Parse(stats[6]),
                Luck = int.Parse(stats[7]),
                EXP = int.Parse(stats[8]),
                Money = int.Parse(stats[9])
            };

            enemies.Add(enemyName, eStats);
        }

        return enemies;
    }
}
