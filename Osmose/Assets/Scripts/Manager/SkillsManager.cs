using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour {
    public static SkillsManager Instance;

    public Skill[] ArenSkills = new Skill[101];
    public Skill[] ReySkills = new Skill[101];
    public Skill[] NaoiseSkills = new Skill[101];

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Skill[] GetCharSkills(protags name) {
        Skill[] skills = new Skill[ArenSkills.Length];
        switch(name) {
            case protags.AREN:
                ArenSkills.CopyTo(skills, 0);
                break;
            case protags.REY:
                ReySkills.CopyTo(skills, 0);
                break;
            case protags.NAOISE:
                NaoiseSkills.CopyTo(skills, 0);
                break;
        }
        return skills;
    }
 }
