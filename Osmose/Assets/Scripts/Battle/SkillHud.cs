using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillHud : MonoBehaviour {
    public CanvasGroup Hud;

    [Header("Skill UI")]
    public Text[] Skills;

    [Header("Description")]
    public Text Description;

    public string currSkill;
    public int skillIndx;

    public int clickedSkill;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
