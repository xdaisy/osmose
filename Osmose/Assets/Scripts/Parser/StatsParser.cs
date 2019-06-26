using System.Collections.Generic;
using UnityEngine;

public struct Stats {
    public int HP, SP, Attack, Defense, MagicDefense, Speed, Luck;
}

public class StatsParser : MonoBehaviour {
    public TextAsset ArenStats;
    public TextAsset ReyStats;
    public TextAsset NaoiseStats;

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
}
