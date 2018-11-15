using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {

    private int level;
    private int currExp;
    private int nextExp;

    private Stat currentHP;
    private Stat currentSP;
    private Stat maxHP;
    private Stat maxSP;
    private Stat attack;
    private Stat defense;
    private Stat speed;
    private Stat luck;

    private Object weapon;
    private Object armor;

    public Character(float hp, float sp, float attack, float defense, float speed, float luck) {
        this.level = 1;
        this.currExp = 0;

        this.currentHP = new Stat(hp);
        this.maxHP = new Stat(hp);
        this.currentSP = new Stat(sp);
        this.maxSP = new Stat(sp);
        this.attack = new Stat(attack);
        this.defense = new Stat(defense);
        this.speed = new Stat(speed);
        this.luck = new Stat(luck);
    }

    /// <summary>
    /// Get character's level
    /// </summary>
    /// <returns></returns>
    public int GetLevel() {
        return this.level;
    }

    /// <summary>
    /// Get character's current amount of HP
    /// </summary>
    /// <returns></returns>
    public float GetCurrentHP() {
        return this.currentHP.Value;
    }

    /// <summary>
    /// Get character's max HP
    /// </summary>
    /// <returns></returns>
    public float GetMaxHP() {
        return this.maxHP.Value;
    }

    /// <summary>
    /// Get character's current amount of SP
    /// </summary>
    /// <returns></returns>
    public float GetCurrentSP() {
        return this.currentSP.Value;
    }

    /// <summary>
    /// Get character's max SP
    /// </summary>
    /// <returns></returns>
    public float GetMaxSP() {
        return this.maxSP.Value;
    }

    /// <summary>
    /// Get character's attack
    /// </summary>
    /// <returns></returns>
    public float GetAttack() {
        return this.attack.Value;
    }

    /// <summary>
    /// Get character's defense
    /// </summary>
    /// <returns></returns>
    public float GetDefense() {
        return this.defense.Value;
    }

    /// <summary>
    /// Get character's speed
    /// </summary>
    /// <returns></returns>
    public float GetSpeed() {
        return this.speed.Value;
    }

    /// <summary>
    /// Get character's luck
    /// </summary>
    /// <returns></returns>
    public float GetLuck() {
        return this.luck.Value;
    }

    /// <summary>
    /// Get the amount of EXP left before level up
    /// </summary>
    /// <returns></returns>
    public int GetExpLeft() {
        return this.nextExp - this.currExp;
    }

    /// <summary>
    /// Increase the experience point gained
    /// </summary>
    /// <param name="exp">Amount of experience points gained</param>
    public void GainExp(int exp) {
        this.currExp += exp;

        if (this.currExp >= this.nextExp) {
            // if current amount of exp surpass amount of exp needed to next level, level up
            levelUp();
        }
    }

    private void levelUp() {

    }
}
