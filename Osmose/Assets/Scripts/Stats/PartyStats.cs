using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType {
    ATTACK = 1,
    DEFENSE = 2,
    SPEED = 3,
    LUCK = 4
}

public static class PartyStats {
    private static Dictionary<string, Character> party;
    private static List<string> currentPartyMembers;

    static PartyStats() {
        party = new Dictionary<string, Character>();
        Character aren = new Character(145, 25, 50, 70, 25, 60, 15);
        Character rey = new Character(125, 50, 35, 40, 50, 45, 20);
        Character naoise = new Character(200, 35, 20, 30, 70, 50, 30);
        party["Aren"] = aren;
        party["Rey"] = rey;
        party["Naoise"] = naoise;

        currentPartyMembers = new List<string>();
        currentPartyMembers.Add("Aren");
    }

    /// <summary>
    /// Get character's current HP
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Character's current HP</returns>
    public static float GetCharacterCurrentHP(string name) {
        return party[name].GetCurrentHP();
    }

    /// <summary>
    /// Get character's max HP
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Character's max HP</returns>
    public static float GetCharacterMaxHP(string name) {
        return party[name].GetMaxHP();
    }

    /// <summary>
    /// Get character's current SP
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Character's current SP</returns>
    public static float GetCharacterCurrentSp(string name) {
        return party[name].GetCurrentSP();
    }

    /// <summary>
    /// Get character's max SP
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Character's max SP</returns>
    public static float GetCharacterMaxSp(string name) {
        return party[name].GetMaxSP();
    }

    /// <summary>
    /// Get character's attack
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Character's attack</returns>
    public static float GetCharacterAttack(string name) {
        return party[name].GetAttack();
    }

    /// <summary>
    /// Get character's defense
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Character's defense</returns>
    public static float GetCharacterDefense(string name) {
        return party[name].GetDefense();
    }

    /// <summary>
    /// Get character's speed
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Character's speed</returns>
    public static float GetCharacterSpeed(string name) {
        return party[name].GetSpeed();
    }

    /// <summary>
    /// Get character's luck
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Character's luck</returns>
    public static float GetCharacterLuck(string name) {
        return party[name].GetLuck();
    }

    /// <summary>
    /// Give the character EXP
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <param name="expPoints">Amount of EXP to gain</param>
    public static void GainExperience(string name, int expPoints) {
        party[name].GainExp(expPoints);
    }

    /// <summary>
    /// Recover HP for the character
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <param name="hitpoints">Amount of HP to recover</param>
    public static void RecoverHP(string name, float hitpoints) {
        party[name].RecoverHP(hitpoints);
    }

    /// <summary>
    /// Recover a percent amount of the character's HP
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <param name="percent">Percent of HP to recover</param>
    public static void RecoverPctHP(string name, float percent) {
        Character character = party[name];
        float hitpoints = (float)Math.Round(character.GetCurrentHP() * percent, 4);
        RecoverHP(name, hitpoints);
    }

    /// <summary>
    /// Recover the character's SP
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <param name="skillpoints">Amount of SP to recover</param>
    public static void RecoverSP(string name, float skillpoints) {
        party[name].RecoverSP(skillpoints);
    }

    /// <summary>
    /// Recover a percent of the character's SP
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <param name="percent">Percent of SP to recover</param>
    public static void RecoverPctSP(string name, float percent) {
        Character character = party[name];
        float skillpoints = (float)Math.Round(character.GetCurrentSP() * percent, 4);
        RecoverSP(name, skillpoints);
    }

    /// <summary>
    /// Buff the stat of the character
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <param name="stat">Type of the stat to buff</param>
    /// <param name="buffer">Amount to buff</param>
    public static void Buff(string name, StatType stat, StatModifier buffer) {
        party[name].Buff(stat, buffer);
    }

    /// <summary>
    /// Remove the buffs from a character
    /// </summary>
    /// <param name="name">Name of the character to remove the buff</param>
    /// <param name="stat">Type of stat of the buff</param>
    /// <param name="buffer">Buff to remove</param>
    public static void RemoveBuff(string name, StatType stat, StatModifier buffer) {
        
    }

    /// <summary>
    /// Debuff the stat of the character
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <param name="stat">Type of the stat to debuff</param>
    /// <param name="debuffer">Amount to debuff</param>
    public static void Debuff(string name, StatType stat, StatModifier debuffer) {
        party[name].Debuff(stat, debuffer);
    }

    /// <summary>
    /// Remove the debuff from a character
    /// </summary>
    /// <param name="name">Name of the character to remove the debuff from</param>
    /// <param name="stat">Type of the stat of the debuff</param>
    /// <param name="debuffer">Debuff to remove</param>
    public static void RemoveDebuff(string name, StatType stat, StatModifier debuffer) {
        party[name].RemoveDebuff(stat, debuffer);
    }

    /// <summary>
    /// Change who's in the party and the order of the party members
    /// </summary>
    /// <param name="party">Array of the members who's currently in the party</param>
    public static void changeMembers(string[] party) {
        currentPartyMembers.Clear();
        foreach (string name in party) {
            currentPartyMembers.Add(name);
        }
    }

    /// <summary>
    /// Return the current party members in order
    /// </summary>
    /// <returns></returns>
    public static List<string> GetCurrentParty() {
        return new List<string>(currentPartyMembers);
    }

    /// <summary>
    /// Return whether or not the character is in the current party
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns></returns>
    public static Boolean IsInParty(string name) {
        return currentPartyMembers.Contains(name);
    }
}