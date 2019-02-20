using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum protags {
    AREN = 13,
    REY = 14,
    NAOISE = 15
}
public static class CharStatsFactory {
    public static CharStats GeneratorChar(protags name) {
        CharStats charStats = null;
        Skill[] skills = new Skill[101];
        switch (name) {
            case protags.AREN:
                charStats = new CharStats(145, 25, 50, 70, 25, 60, 15, skills);
                break;
            case protags.REY:
                charStats = new CharStats(125, 50, 35, 40, 50, 45, 20, skills);
                break;
            case protags.NAOISE:
                charStats = new CharStats(200, 35, 20, 30, 70, 50, 30, skills);
                break;
        }
        return charStats;
    }
}
