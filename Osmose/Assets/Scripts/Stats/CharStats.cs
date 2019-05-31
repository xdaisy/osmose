using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharStats {
    public int Level;
    public int CurrExp;
    public int NextExp;
    public List<Skill> Skills; // learned skills
    private Skill[] skillsToLearn; // skills need to learn

    public int CurrHP;
    public int CurrSP;
    public int MaxHP;
    public int MaxSP;
    public int Attack;
    public int Defense;
    public int MagicDefense;
    public int Speed;
    public int Luck;

    public string Weapon;
    public string Armor;

    public int WeaponAttack;
    public int ArmorDefense;

    public bool IsDefending;

    // modifiers for buffing/debuffing
    public float AttkMod = 1f;
    public float DefMod = 1f;
    public float MDefMod = 1f;
    public float SpdMod = 1f;
    public float LckMod = 1f;

    // constructor for creating character stats when start new game
    public CharStats(int hp, int sp, int attack, int defense, int magicDefense, int speed, int luck, Skill[] skills) {
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

        this.Weapon = "";
        this.WeaponAttack = 0;
        this.Armor = "";
        this.ArmorDefense = 0;

        Skills = new List<Skill>();
        skillsToLearn = skills;
        // if have skill at level 1 or level 0, put in learned skill list
        //if (skills[Level] != null) {
        //    Skills.Add(skillsToLearn[Level]);
        //}
        for (int i = 0; i <= Level; i++) {
            if (skillsToLearn[i] != null) {
                Skills.Add(skillsToLearn[i]);
            }
        }
    }

    // load the character's stats
    public void LoadStats(SaveStats stats) {
        this.Level = stats.Level;
        this.CurrExp = stats.CurrExp;
        this.NextExp = stats.NextExp;

        this.CurrHP = stats.CurrHP;
        this.MaxHP = stats.MaxHP;
        this.CurrSP = stats.CurrSP;
        this.MaxSP = stats.MaxSP;
        this.Attack = stats.Attack;
        this.Defense = stats.Defense;
        this.MagicDefense = stats.MagicDefense;
        this.Speed = stats.Speed;
        this.Luck = stats.Luck;

        this.Weapon = stats.Weapon;
        this.WeaponAttack = stats.WeaponAttack;
        this.Armor = stats.Armor;
        this.ArmorDefense = stats.ArmorDefense;

        for (int i = 2; i <= this.Level; i++) {
            if (skillsToLearn[i] != null) {
                Skills.Add(skillsToLearn[i]);
            }
        }
    }

    // get the stats of the character
    public SaveStats GetStats() {
        SaveStats stats = new SaveStats {
            Level = this.Level,
            CurrExp = this.CurrExp,
            NextExp = this.NextExp,

            CurrHP = this.CurrHP,
            MaxHP = this.MaxHP,
            CurrSP = this.CurrSP,
            MaxSP = this.MaxSP,
            Attack = this.Attack,
            Defense = this.Defense,
            MagicDefense = this.MagicDefense,
            Speed = this.Speed,
            Luck = this.Luck,

            Weapon = this.Weapon,
            WeaponAttack = this.WeaponAttack,
            Armor = this.Armor,
            ArmorDefense = this.ArmorDefense
        };
        return stats;
    }

    // get the skill with the skillName
    public Skill GetSkill(string skillName) {
        for (int i = 0; i < Skills.Count; i++) {
            if (Skills[i].SkillName == skillName) {
                return Skills[i];
            }
        }
        return null;
    }

    public int GetExpLeft() {
        return this.NextExp - this.CurrExp;
    }
    
    public bool GainExp(int exp) {
        this.CurrExp += exp;

        if (this.CurrExp >= this.NextExp) {
            // if current amount of exp surpass amount of exp needed to next level, level up
            levelUp();
            this.NextExp += GameManager.Instance.Party.GetExpToNextLvl(Level); // increase how much total exp needed to go next level
            return true;
        }
        return false;
    }

    private void levelUp() {
        Level++;

        MaxHP += Mathf.RoundToInt(Mathf.Min(MaxHP * 1.025f, 500));
        CurrHP = MaxHP;

        MaxSP += Mathf.RoundToInt(Mathf.Min(MaxSP * 1.025f, 300f));
        CurrSP = MaxSP;

        Attack += Mathf.RoundToInt(Mathf.Min(Attack * 1.025f, 200));

        Defense += Mathf.RoundToInt(Defense * 1.025f);

        MagicDefense += Mathf.RoundToInt(Mathf.Min(MagicDefense * 1.025f, 200f));

        Speed += Mathf.RoundToInt(Mathf.Min(Speed * 1.025f, 200f));

        Luck += Mathf.RoundToInt(Mathf.Min(Luck * 1.025f, 200f));

        if (skillsToLearn[Level] != null) {
            // learn skill
            Skills.Add(skillsToLearn[Level]);
        }
    }
}

[Serializable]
public class SaveStats {
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

    public string Weapon;
    public string Armor;

    public int WeaponAttack;
    public int ArmorDefense;
}