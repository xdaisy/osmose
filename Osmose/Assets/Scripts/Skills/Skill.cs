using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {
    [Header("General Info")]
    public string SkillName;
    public int Cost;
    public string Description;

    [Header("Skill Type")]
    public bool IsPhyAttk;
    public bool IsMagAttk;
    public bool IsHeal;

    [Header("Skill Info")]
    public float PercentValue;

    public float UseSkill(string charName) {
        // reduce character's sp for using the skill
        GameManager.Instance.Party.CharUseSkill(charName, Cost);
        if (IsPhyAttk || IsMagAttk) {
            return GameManager.Instance.Party.GetCharacterAttack(charName) * PercentValue;
        }

        if (IsHeal) {
            GameManager.Instance.Party.RecoverPctHP(charName, PercentValue);
            return PercentValue;
        }

        return 0f;
    }
}
