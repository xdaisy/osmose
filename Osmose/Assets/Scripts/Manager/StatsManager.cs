using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour {
    public static StatsManager Instance;
    public StatsParser parser;

    private Dictionary<string, Dictionary<string, int[]>> protagStats;

    private Dictionary<string, Dictionary<string, int>> enemyStats;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start() {
        protagStats = new Dictionary<string, Dictionary<string, int[]>>();
        protagStats.Add(Constants.AREN, parser.GetProtagStats(Constants.AREN));

        enemyStats = new Dictionary<string, Dictionary<string, int>>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
