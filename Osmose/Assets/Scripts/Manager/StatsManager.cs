using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour {
    public static StatsManager Instance;
    public StatsParser Parser;

    private Dictionary<string, Stats[]> protagStats;

    private Dictionary<string, EnemyStats> enemyStats;

    // Start is called before the first frame update
    void Start() {
        if (Instance == null) {
            Instance = this;
            protagStats = new Dictionary<string, Stats[]>();
            protagStats.Add(Constants.AREN, Parser.GetProtagStats(Constants.AREN));
            protagStats.Add(Constants.REY, Parser.GetProtagStats(Constants.REY));
            protagStats.Add(Constants.NAOISE, Parser.GetProtagStats(Constants.NAOISE));

            enemyStats = Parser.GetEnemyStats();
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Get the stats of a level for a character
    /// </summary>
    /// <param name="charName">Name of the character</param>
    /// <param name="level">Level of the stats</param>
    /// <returns>Stats at the level for a character</returns>
    public Stats GetCharStatsAt(string charName, int level) {
        if (!protagStats.ContainsKey(charName)) {
            // return a new stat aka 0 if character is not a protag
            return new Stats();
        }
        // otherwise return the stat of the character at level if it exists
        return protagStats[charName][level];
    }

    /// <summary>
    /// Get the stats of an enemy
    /// </summary>
    /// <param name="enemyName">Name of the enemy</param>
    /// <returns>Stats of the enemy</returns>
    public EnemyStats GetEnemyStats(string enemyName) {
        if (!enemyStats.ContainsKey(enemyName)) {
            return new EnemyStats();
        }
        return enemyStats[enemyName];
    }
}
