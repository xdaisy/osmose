using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyStats {
    private Dictionary<string, Character> party = new Dictionary<string, Character>();

    public float GetCharacterCurrentHP(string name) {
        return party[name].GetCurrentHP();
    }

    public float GetCharacterMaxHP(string name) {
        return party[name].GetMaxHP();
    }

    public float GetCharacterCurrentSp(string name) {
        return party[name].GetCurrentSP();
    }

    public float GetCharacterMaxSp(string name) {
        return party[name].GetMaxSP();
    }

    public float GetCharacterAttack(string name) {
        return party[name].GetAttack();
    }

    public float GetCharacterDefense(string name) {
        return party[name].GetDefense();
    }

    public float GetCharacterSpeed(string name) {
        return party[name].GetSpeed();
    }

    public float GetCharacterLuck(string name) {
        return party[name].GetLuck();
    }

    public void GainExperience(string[] party, int expPoints) {
        foreach(string name in party) {
            this.party[name].GainExp(expPoints);
        }
    }

    public void RecoverHP(string[] characters, float hitpoints) {
        foreach(string name in characters) {
            party[name].RecoverHP(hitpoints);
        }
    }
}
