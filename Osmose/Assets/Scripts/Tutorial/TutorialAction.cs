using UnityEngine;

/// <summary>
/// Class that keeps track of the Battle Tutorial action
/// </summary>
[CreateAssetMenu(menuName = "TutorialAction")]
public class TutorialAction : ScriptableObject {
    [TextArea(10, 14)] [SerializeField] private string text; // if not enemy, show text
    [SerializeField] private bool attackInteractable; // else if true, can attack
    [SerializeField] private bool skillsInteractable; // else if true, can use skills
    [SerializeField] private bool useFirstSkill; // flag for if can use first skill
    [SerializeField] private bool useSecondSkill; // flag for if can use second skill
    [SerializeField] private bool enemyAttack; // else if true, enemy's turn

    /// <summary>
    /// Get the text of the action
    /// </summary>
    /// <returns>Text to be displayed</returns>
    public string GetText() {
        return text;
    }

    /// <summary>
    /// Get flag on whether or not attack can be done
    /// </summary>
    /// <returns>True if can attack, false otherwise</returns>
    public bool GetAttackInteractable() {
        return attackInteractable;
    }

    /// <summary>
    /// Get flag on whether or not can use skills
    /// </summary>
    /// <returns>True if can use skills, false otherwise</returns>
    public bool GetSkillsInteractable() {
        return skillsInteractable;
    }

    /// <summary>
    /// Get flag on whether can use the first skill
    /// </summary>
    /// <returns>True if can use the first skill, false otherwise</returns>
    public bool GetUseFirstSkill() {
        return useFirstSkill;
    }

    /// <summary>
    /// Get flag on whether can use the second skill
    /// </summary>
    /// <returns>True if can use the second skill, false otherwise</returns>
    public bool GetUseSecondSkill() {
        return useSecondSkill;
    }

    /// <summary>
    /// Get flag on whether or not it's the enemy's turn
    /// </summary>
    /// <returns>True if it's the enemy's turn, false otherwise</returns>
    public bool GetEnemyAttack() {
        return enemyAttack;
    }
}
