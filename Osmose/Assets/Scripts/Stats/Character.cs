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
        this.nextExp = 10;

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

    /// <summary>
    /// Recover character's HP
    /// </summary>
    /// <param name="hitpoint">Amount of HP to recover</param>
    public void RecoverHP(float hitpoints) {
        if (GetCurrentHP() + hitpoints >= GetMaxHP()) {
            currentHP.BaseValue = GetMaxHP();
        } else {
            currentHP.BaseValue += hitpoints;
        }
    }

    /// <summary>
    /// Recover character's SP
    /// </summary>
    /// <param name="skillpoints">Amount of SP to recover</param>
    public void RecoverSP(float skillpoints) {
        if (GetCurrentSP() + skillpoints >= GetMaxSP()) {
            currentSP.BaseValue = GetMaxHP();
        } else {
            currentSP.BaseValue += skillpoints;
        }
    }

    /// <summary>
    /// Buff the stat
    /// </summary>
    /// <param name="stat">Stat to buff</param>
    /// <param name="buffer">Amount to buff</param>
    public void Buff(StatType stat, StatModifier buffer) {
        if (stat == StatType.ATTACK) {
            attack.AddModifier(buffer);
        } else if (stat == StatType.DEFENSE) {
            defense.AddModifier(buffer);
        } else if (stat == StatType.LUCK) {
            luck.AddModifier(buffer);
        } else if (stat == StatType.SPEED) {
            speed.AddModifier(buffer);
        }
    }
    
    /// <summary>
    /// Remove the buff from the character
    /// </summary>
    /// <param name="stat">Stat to remove the buff</param>
    /// <param name="buffer">Amount of buff to remove</param>
    public void RemoveBuff(StatType stat, StatModifier buffer) {
        if (stat == StatType.ATTACK) {
            attack.RemoveModifier(buffer);
        } else if (stat == StatType.DEFENSE) {
            defense.RemoveModifier(buffer);
        } else if (stat == StatType.LUCK) {
            luck.RemoveModifier(buffer);
        } else if (stat == StatType.SPEED) {
            speed.RemoveModifier(buffer);
        }
    }

    /// <summary>
    /// Debuff the stat
    /// </summary>
    /// <param name="stat">Stat to debuff</param>
    /// <param name="debuffer">Amount to debuff</param>
    public void Debuff(StatType stat, StatModifier debuffer) {
        if (stat == StatType.ATTACK) {
            attack.AddModifier(debuffer);
        } else if (stat == StatType.DEFENSE) {
            defense.AddModifier(debuffer);
        } else if (stat == StatType.LUCK) {
            luck.AddModifier(debuffer);
        } else if (stat == StatType.SPEED) {
            speed.AddModifier(debuffer);
        }
    }

    /// <summary>
    /// Remove the debuff from the character
    /// </summary>
    /// <param name="stat">Type of stat of the debuff</param>
    /// <param name="debuffer">Debuff to remove</param>
    public void RemoveDebuff(StatType stat, StatModifier debuffer) {
        if (stat == StatType.ATTACK) {
            attack.RemoveModifier(debuffer);
        } else if (stat == StatType.DEFENSE) {
            defense.RemoveModifier(debuffer);
        } else if (stat == StatType.LUCK) {
            luck.RemoveModifier(debuffer);
        } else if (stat == StatType.SPEED) {
            speed.RemoveModifier(debuffer);
        }
    }

    private void levelUp() {

    }
}
