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

public class PartyStats {
    private Dictionary<string, Character> party = new Dictionary<string, Character>();

    /// <summary>
    /// Get character's current HP
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Character's current HP</returns>
    public float GetCharacterCurrentHP(string name) {
        return party[name].GetCurrentHP();
    }

    /// <summary>
    /// Get character's max HP
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Character's max HP</returns>
    public float GetCharacterMaxHP(string name) {
        return party[name].GetMaxHP();
    }

    /// <summary>
    /// Get character's current SP
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Character's current SP</returns>
    public float GetCharacterCurrentSp(string name) {
        return party[name].GetCurrentSP();
    }

    /// <summary>
    /// Get character's max SP
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Character's max SP</returns>
    public float GetCharacterMaxSp(string name) {
        return party[name].GetMaxSP();
    }

    /// <summary>
    /// Get character's attack
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Character's attack</returns>
    public float GetCharacterAttack(string name) {
        return party[name].GetAttack();
    }

    /// <summary>
    /// Get character's defense
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Character's defense</returns>
    public float GetCharacterDefense(string name) {
        return party[name].GetDefense();
    }

    /// <summary>
    /// Get character's speed
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Character's speed</returns>
    public float GetCharacterSpeed(string name) {
        return party[name].GetSpeed();
    }

    /// <summary>
    /// Get character's luck
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <returns>Character's luck</returns>
    public float GetCharacterLuck(string name) {
        return party[name].GetLuck();
    }

    /// <summary>
    /// Give the character EXP
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <param name="expPoints">Amount of EXP to gain</param>
    public void GainExperience(string name, int expPoints) {
        party[name].GainExp(expPoints);
    }

    /// <summary>
    /// Recover HP for the character
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <param name="hitpoints">Amount of HP to recover</param>
    public void RecoverHP(string name, float hitpoints) {
        party[name].RecoverHP(hitpoints);
    }

    /// <summary>
    /// Recover a percent amount of the character's HP
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <param name="percent">Percent of HP to recover</param>
    public void RecoverPctHP(string name, float percent) {
        Character character = party[name];
        float recoverHP = (float)Math.Round(character.GetCurrentHP() * percent, 4);
        character.RecoverHP(recoverHP);
    }

    /// <summary>
    /// Recover the character's SP
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <param name="skillpoints">Amount of SP to recover</param>
    public void RecoverSP(string name, float skillpoints) {
        party[name].RecoverSP(skillpoints);
    }

    /// <summary>
    /// Recover a percent of the character's SP
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <param name="percent">Percent of SP to recover</param>
    public void RecoverPctSP(string name, float percent) {
        Character character = party[name];
        float recoverSP = (float)Math.Round(character.GetCurrentSP() * percent, 4);
    }

    /// <summary>
    /// Buff the stat of the character
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <param name="buffer">Amount to buff</param>
    public void Buff(string name, StatModifier buffer) {

    }

    /// <summary>
    /// Debuff the stat of the character
    /// </summary>
    /// <param name="name">Name of the character</param>
    /// <param name="debuffer">Amount to debuff</param>
    public void Debuff(string name, StatModifier debuffer) {

    }
}