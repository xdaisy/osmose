using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats {

    public int Level;
    public int CurrExp;
    public int NextExp;

    public int CurrHP;
    public int CurrSP;
    public int MaxHP;
    public int MaxSP;
    public int Attack;
    public int Defense;
    public int MagicDefense;
    public int Speed;
    public int Luck;

    public int WeaponAttack;
    public int ArmorDefense;

    public bool IsDefending;

    // modifiers for buffing
    public float AttackModifier = 1f;
    public float DefenseModifier = 1f;
    public float MagicDefenseModifier = 1f;
    public float SpeedModifier = 1f;
    public float LuckModifier = 1f;

    public CharStats(int hp, int sp, int attack, int defense, int magicDefense, int speed, int luck) {
        this.Level = 1;
        this.CurrExp = 0;
        this.NextExp = 10;
        this.IsDefending = false;

        this.CurrHP = hp;
        this.MaxHP = hp;
        this.CurrSP = sp;
        this.MaxSP = sp;
        this.Attack = attack;
        this.Defense = defense;
        this.MagicDefense = magicDefense;
        this.Speed = speed;
        this.Luck = luck;
    }
    
    public int GetExpLeft() {
        return this.NextExp - this.CurrExp;
    }
    
    public void GainExp(int exp) {
        this.CurrExp += exp;

        if (this.CurrExp >= this.NextExp) {
            // if current amount of exp surpass amount of exp needed to next level, level up
            levelUp();
            this.NextExp += GameManager.Instance.Party.GetExpToNextLvl(Level); // increase how much total exp needed to go next level
        }
    }

    private void levelUp() {
        Level++;

        MaxHP += Mathf.RoundToInt(MaxHP * 1.05f);
        CurrHP = MaxHP;

        MaxSP += Mathf.RoundToInt(MaxSP * 1.05f);
        CurrSP = MaxSP;

        Attack += Mathf.RoundToInt(Attack * 1.05f);

        Defense += Mathf.RoundToInt(Defense * 1.05f);

        MagicDefense += Mathf.RoundToInt(MagicDefense * 1.05f);

        Speed += Mathf.RoundToInt(Speed * 1.05f);

        Luck += Mathf.RoundToInt(Luck * 1.05f);
    }
}
