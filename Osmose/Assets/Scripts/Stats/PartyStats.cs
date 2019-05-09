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
    private int baseExp = 101;

    private int[] expToNextLvl;

    public PartyStats() {
        party = new Dictionary<string, CharStats>();
        CharStats aren = CharStatsFactory.GeneratorChar(Constants.AREN);
        CharStats rey = CharStatsFactory.GeneratorChar(Constants.REY);
        CharStats naoise = CharStatsFactory.GeneratorChar(Constants.NAOISE);
        party[Constants.AREN] = aren;
        party[Constants.REY] = rey;
        party[Constants.NAOISE] = naoise;
        currentPartyMembers = new List<string>();
        currentPartyMembers.Add(Constants.AREN);
        currentPartyMembers.Add(Constants.REY);
        currentPartyMembers.Add(Constants.NAOISE);

        expToNextLvl = new int[maxLevel];
        expToNextLvl[1] = baseExp;
        for (int i = 2; i < expToNextLvl.Length; i++) {
            expToNextLvl[i] = Mathf.FloorToInt(expToNextLvl[i - 1] * 1.05f);
        }
    }

    // get character skill at indx
    public Skill GetCharSkillAt(string name, int indx) {
        if (indx >= party[name].Skills.Count) {
            return null;
        }
        return party[name].Skills[indx];
    }

    // get skill from character with the skillName
    public Skill GetCharSkill(string name, string skillName) {
        return party[name].GetSkill(skillName);
    }

    // deplete sp when char use skill
    public void CharUseSkill(string name, int spCost) {
        party[name].CurrSP -= spCost;
    }
    
    public bool IsDefending(string name) {
        return party[name].IsDefending;
    }
    
    public void SetDefending(string name, bool isDefending) {
        party[name].IsDefending = isDefending;
    }
    
    public int GetCharCurrHP(string name) {
        return party[name].CurrHP;
    }
    
    public int GetCharMaxHP(string name) {
        return party[name].MaxHP;
    }
    
    public int GetCharCurrSP(string name) {
        return party[name].CurrSP;
    }
    
    public int GetCharMaxSP(string name) {
        return party[name].MaxSP;
    }
    
    public int GetCharAttk(string name) {
        // get total attack
        return Mathf.RoundToInt((party[name].Attack + party[name].WeaponAttack) * party[name].AttkMod);
    }
    
    public int GetCharDef(string name) {
        // get total defense
        return Mathf.RoundToInt((party[name].Defense + party[name].ArmorDefense) * party[name].DefMod);
    }
    
    public int GetCharMagDef(string name) {
        // get magic defense
        return Mathf.RoundToInt(party[name].MagicDefense * party[name].MDefMod);
    }
    
    public int GetCharSpd(string name) {
        // get speed
        return Mathf.RoundToInt(party[name].Speed * party[name].SpdMod);
    }
    
    public int GetCharLck(string name) {
        // get luck
        return Mathf.RoundToInt(party[name].Luck * party[name].LckMod);
    }

    public int GetCharLvl(string name) {
        return party[name].Level;
    }

    public int GetCharCurrEXP(string name) {
        return party[name].CurrExp;
    }
    
    public int GetCharEXPtoNextLvl(string name) {
        return party[name].NextExp;
    }
    
    public void GainExperience(int expPoints) {
        foreach (string name in currentPartyMembers) {
            party[name].GainExp(expPoints);
        }
    }
    
    public int GetExpToNextLvl(int level) {
        return expToNextLvl[level];
    }

    public bool IsAlive(string name) {
        return party[name].CurrHP > 0;
    }
    
    public void RecoverHP(string name, int hitpoints) {
        party[name].CurrHP = Math.Min(party[name].CurrHP + hitpoints, party[name].MaxHP);
    }
    
    public void RecoverPctHP(string name, float percent) {
        CharStats character = party[name];
        int hitpoints = Mathf.RoundToInt(character.MaxHP * percent);
        RecoverHP(name, hitpoints);
    }
    
    public void RecoverSP(string name, int skillpoints) {
        party[name].CurrSP = Math.Min(party[name].CurrSP + skillpoints, party[name].MaxSP);
    }
    
    public void RecoverPctSP(string name, float percent) {
        CharStats character = party[name];
        int skillpoints = Mathf.RoundToInt(character.MaxSP * percent);
        RecoverSP(name, skillpoints);
    }

    public string GetWeapon(string name) {
        return party[name].Weapon;
    }

    public int GetWeaponStat(string name) {
        return party[name].WeaponAttack;
    }

    public void EquipWeapon(string name, string weapon, int weaponStr) {
        party[name].Weapon = weapon;
        party[name].WeaponAttack = weaponStr;
    }

    public string GetArmor(string name) {
        return party[name].Armor;
    }

    public int GetArmorStat(string name) {
        return party[name].ArmorDefense;
    }

    public void EquipArmor(string name, string armor, int armorDefn) {
        party[name].Armor = armor;
        party[name].ArmorDefense = armorDefn;
    }

    // permanently increase stat
    public void IncreaseStat(string name, StatType statType, int amtToIncrease) {
        switch(statType) {
            case StatType.ATTACK:
                party[name].Attack += amtToIncrease;
                break;
            case StatType.DEFENSE:
                party[name].Defense += amtToIncrease;
                break;
            case StatType.MAGICDEFENSE:
                party[name].MagicDefense += amtToIncrease;
                break;
            case StatType.SPEED:
                party[name].Speed += amtToIncrease;
                break;
            case StatType.LUCK:
                party[name].Luck += amtToIncrease;
                break;
        }
    }

    // increase stat modifier
    public void BuffStats(string name, StatType statType, float amtToIncrease) {
        switch(statType) {
            case StatType.ATTACK:
                party[name].AttkMod += amtToIncrease;
                break;
            case StatType.DEFENSE:
                party[name].DefMod += amtToIncrease;
                break;
            case StatType.MAGICDEFENSE:
                party[name].MDefMod += amtToIncrease;
                break;
            case StatType.SPEED:
                party[name].SpdMod += amtToIncrease;
                break;
            case StatType.LUCK:
                party[name].LckMod += amtToIncrease;
                break;
        }
    }

    // decrease stat modifier
    public void DebuffStats(string name, StatType statType, float amtToDecrease) {
        switch(statType) {
            case StatType.ATTACK:
                party[name].AttkMod -= amtToDecrease;
                break;
            case StatType.DEFENSE:
                party[name].DefMod -= amtToDecrease;
                break;
            case StatType.MAGICDEFENSE:
                party[name].MDefMod -= amtToDecrease;
                break;
            case StatType.SPEED:
                party[name].SpdMod -= amtToDecrease;
                break;
            case StatType.LUCK:
                party[name].LckMod -= amtToDecrease;
                break;
        }
    }

    // reset all modifiers
    public void ResetStatsModifier() {
        foreach (string name in currentPartyMembers) {
            party[name].AttkMod = 1f;
            party[name].DefMod = 1f;
            party[name].MDefMod = 1f;
            party[name].SpdMod = 1f;
            party[name].LckMod = 1f;
        }
    }
    
    public void ChangeMembers(List<string> party) {
        currentPartyMembers = party;
    }

    // fully recover the current party
    // use when party is defeated and is teleported back to town
    public void RecoverParty() {
        foreach (string member in currentPartyMembers) {
            party[member].CurrHP = party[member].MaxHP;
            party[member].CurrSP = party[member].MaxSP;
        }
    }
    
    public List<string> GetCurrentParty() {
        return new List<string>(currentPartyMembers);
    }
    
    public Boolean IsInParty(string name) {
        return currentPartyMembers.Contains(name);
    }

    public void DealtDamage(string name, int damage) {
        int currentHP = GetCharCurrHP(name) - damage;
        currentHP = Math.Max(currentHP, 0); // hp can't go below 0
        party[name].CurrHP = currentHP;
    }

    // get a specific character's stats
    public CharStats GetCharacterStats(string name) {
        return party[name];
    }

    // load a specific character's stats
    public void LoadCharStats(string name, CharStats stats) {
        party[name].LoadStats(stats);
    }
}