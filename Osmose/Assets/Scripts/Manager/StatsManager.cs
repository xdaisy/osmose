using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour {
    public static StatsManager Instance;
    public StatsParser parser;

    private Dictionary<string, Stats[]> protagStats;

    private Dictionary<string, Stats> enemyStats;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            protagStats = new Dictionary<string, Stats[]>();
            protagStats.Add(Constants.AREN, parser.GetProtagStats(Constants.AREN));
            protagStats.Add(Constants.REY, parser.GetProtagStats(Constants.REY));
            protagStats.Add(Constants.NAOISE, parser.GetProtagStats(Constants.NAOISE));

            enemyStats = new Dictionary<string, Stats>();
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start() {
    }

    public Stats GetCharStatsAt(string charName, int level) {
        if (!protagStats.ContainsKey(charName)) {
            // return a new stat aka 0 if character is not a protag
            return new Stats();
        }
        // otherwise return the stat of the character at level if it exists
        return protagStats[charName][level];
    }
}
