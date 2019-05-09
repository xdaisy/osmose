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

    public Skill[] GetCharSkills(string name) {
        Skill[] skills = new Skill[ArenSkills.Length];
        switch(name) {
            case Constants.AREN:
                ArenSkills.CopyTo(skills, 0);
                break;
            case Constants.REY:
                ReySkills.CopyTo(skills, 0);
                break;
            case Constants.NAOISE:
                NaoiseSkills.CopyTo(skills, 0);
                break;
        }
        return skills;
    }
 }
