using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {
    [Header("General Info")]
    public string SkillName;
    public int Cost;
    public string Description;
    public bool UseInShift;
    public bool UseOnSelf;
    public bool UseOnEnemy;

    [Header("Skill Type")]
    public bool IsPhyAttk;
    public bool IsMagAttk;
    public bool IsHeal;
    public bool BuffAttk, BuffDef, BuffMDef, BuffSpd, BuffLck;
    public bool DebuffAttk, DebuffDef, DebuffMDef, DebuffSpd, DebuffLck;

    [Header("Skill Info")]
    public float PercentValue;

    // for attacking or using skill on self
    public float UseSkill(string charName) {
        // reduce character's sp for using the skill
        GameManager.Instance.Party.CharUseSkill(charName, Cost);
        if (IsPhyAttk || IsMagAttk) {
            return GameManager.Instance.Party.GetCharAttk(charName) * PercentValue;
        }

        // heal char
        if (IsHeal) {
            GameManager.Instance.Party.RecoverPctHP(charName, PercentValue);
        }

        // buff char
        if (BuffAttk) {
            GameManager.Instance.Party.BuffStats(charName, StatType.ATTACK, PercentValue);
        }
        if (BuffDef) {
            GameManager.Instance.Party.BuffStats(charName, StatType.DEFENSE, PercentValue);
        }
        if (BuffMDef) {
            GameManager.Instance.Party.BuffStats(charName, StatType.MAGICDEFENSE, PercentValue);
        }
        if (BuffSpd) {
            GameManager.Instance.Party.BuffStats(charName, StatType.SPEED, PercentValue);
        }
        if (BuffLck) {
            GameManager.Instance.Party.BuffStats(charName, StatType.LUCK, PercentValue);
        }

        // debuff char
        if (DebuffAttk) {
            GameManager.Instance.Party.DebuffStats(charName, StatType.ATTACK, PercentValue);
        }
        if (DebuffDef) {
            GameManager.Instance.Party.DebuffStats(charName, StatType.DEFENSE, PercentValue);
        }
        if (DebuffMDef) {
            GameManager.Instance.Party.DebuffStats(charName, StatType.MAGICDEFENSE, PercentValue);
        }
        if (DebuffSpd) {
            GameManager.Instance.Party.DebuffStats(charName, StatType.SPEED, PercentValue);
        }
        if (DebuffLck) {
            GameManager.Instance.Party.DebuffStats(charName, StatType.LUCK, PercentValue);
        }

        return 0f;
    }

    // for using skill on party member
    public void UseSkill(string charUsingSkill, string charUsingSkillOn) {
        // reduce character's sp for using the skill
        GameManager.Instance.Party.CharUseSkill(charUsingSkill, Cost);

        if (IsHeal) {
            GameManager.Instance.Party.RecoverPctHP(charUsingSkillOn, PercentValue);
        }
    }
}
