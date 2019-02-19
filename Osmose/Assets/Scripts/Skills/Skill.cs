using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("General Info")]
    public string SkillName;
    public bool IsPhyAttk;
    public bool IsMagAttk;

    [Header("Skill Info")]
    public float PercentValue;

    public void UseSkill(string charUsingSkill) {

    }
}
