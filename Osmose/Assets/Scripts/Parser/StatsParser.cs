using System.Collections.Generic;
using UnityEngine;

public class StatsParser : MonoBehaviour {
    public TextAsset ArenStats;

    public Dictionary<string, int[]> GetProtagStats(string name) {
        Dictionary<string, int[]> stats = new Dictionary<string, int[]> {
            { Constants.HP, new int[101] },
            { Constants.SP, new int[101] },
            { Constants.ATTACK, new int[101] },
            { Constants.DEFENSE, new int[101] },
            { Constants.MAGIC_DEFENSE, new int[101] },
            { Constants.SPEED, new int[101] },
            { Constants.LUCK, new int[101] }
        };

        string[] charStats = null;

        switch(name) {
            case Constants.AREN:
                charStats = ArenStats.text.Split('\n');
                break;
            case Constants.REY:
                break;
            case Constants.NAOISE:
                break;
        }


        for (int i = 1; i < charStats.Length - 1; i++) {
            string[] levelStats = charStats[i].Split(',');

            int level = int.Parse(levelStats[0]);

            stats[Constants.HP][level] = int.Parse(levelStats[1]);
            stats[Constants.SP][level] = int.Parse(levelStats[2]);
            stats[Constants.ATTACK][level] = int.Parse(levelStats[3]);
            stats[Constants.DEFENSE][level] = int.Parse(levelStats[4]);
            stats[Constants.MAGIC_DEFENSE][level] = int.Parse(levelStats[5]);
            stats[Constants.SPEED][level] = int.Parse(levelStats[6]);
            stats[Constants.LUCK][level] = int.Parse(levelStats[7]);
        }
        return stats;
    }
}
