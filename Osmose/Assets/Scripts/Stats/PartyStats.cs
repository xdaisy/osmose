using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType {
    ATTACK = 1,
    DEFENSE = 2,
    MAGICDEFENSE = 3,
    SPEED = 4,
    LUCK = 5
}

public class PartyStats {
    private Dictionary<string, CharStats> party;
    private List<string> currentPartyMembers;

    private int maxLevel = 100;
    private int baseExp = 100;

    private int[] expToNextLvl;

    public PartyStats() {
        party = new Dictionary<string, CharStats>();
        CharStats aren = new CharStats(145, 25, 50, 70, 25, 60, 15);
        CharStats rey = new CharStats(125, 50, 35, 40, 50, 45, 20);
        CharStats naoise = new CharStats(200, 35, 20, 30, 70, 50, 30);
        party["Aren"] = aren;
        party["Rey"] = rey;
        party["Naoise"] = naoise;

        currentPartyMembers = new List<string>();
        currentPartyMembers.Add("Aren");

        expToNextLvl = new int[maxLevel];
        expToNextLvl[1] = baseExp;
        for (int i = 2; i < expToNextLvl.Length; i++) {
            expToNextLvl[i] = Mathf.FloorToInt(expToNextLvl[i - 1] * 1.05f);
        }
    }
    
    public bool IsDefending(string name) {
        return party[name].IsDefending;
    }
    
    public void SetDefending(string name, bool isDefending) {
        party[name].IsDefending = isDefending;
    }
    
    public float GetCharacterCurrentHP(string name) {
        return party[name].CurrHP;
    }
    
    public float GetCharacterMaxHP(string name) {
        return party[name].MaxHP;
    }
    
    public float GetCharacterCurrentSp(string name) {
        return party[name].CurrSP;
    }
    
    public float GetCharacterMaxSp(string name) {
        return party[name].MaxSP;
    }
    
    public float GetCharacterAttack(string name) {
        return party[name].Attack;
    }
    
    public float GetCharacterDefense(string name) {
        return party[name].Defense;
    }
    
    public float GetCharacterSpeed(string name) {
        return party[name].Speed;
    }
    
    public float GetCharacterLuck(string name) {
        return party[name].Luck;
    }
    
    public void GainExperience(int expPoints) {
        foreach (string name in currentPartyMembers) {
            party[name].GainExp(expPoints);
        }
    }
    
    public int GetExpToNextLvl(int level) {
        return expToNextLvl[level];
    }
    
    public void RecoverHP(string name, int hitpoints) {
        party[name].CurrHP = Math.Min(party[name].CurrHP + hitpoints, party[name].MaxHP);
    }
    
    public void RecoverPctHP(string name, float percent) {
        CharStats character = party[name];
        int hitpoints = Mathf.RoundToInt(character.CurrHP * percent);
        RecoverHP(name, hitpoints);
    }
    
    public void RecoverSP(string name, int skillpoints) {
        party[name].CurrSP = Math.Min(party[name].CurrSP + skillpoints, party[name].MaxSP);
    }
    
    public void RecoverPctSP(string name, float percent) {
        CharStats character = party[name];
        int skillpoints = Mathf.RoundToInt(character.CurrSP * percent);
        RecoverSP(name, skillpoints);
    }

    public void ClearStatsModifier() {
        foreach (string name in currentPartyMembers) {
            party[name].AttackModifier = 1f;
            party[name].DefenseModifier = 1f;
            party[name].MagicDefenseModifier = 1f;
            party[name].SpeedModifier = 1f;
            party[name].LuckModifier = 1f;
        }
    }
    
    public void ChangeMembers(string[] party) {
        currentPartyMembers.Clear();
        foreach (string name in party) {
            currentPartyMembers.Add(name);
        }
    }
    
    public List<string> GetCurrentParty() {
        return new List<string>(currentPartyMembers);
    }
    
    public Boolean IsInParty(string name) {
        return currentPartyMembers.Contains(name);
    }
}