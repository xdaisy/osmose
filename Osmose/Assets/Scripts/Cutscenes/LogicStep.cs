using UnityEngine;

/// <summary>
/// Class for keeping track of the information for a step in the Logic System
/// </summary>
[CreateAssetMenu(menuName = "LogicStep")]
public class LogicStep : ScriptableObject {
    [SerializeField] private string[] dialogue;
    [SerializeField] private string question;
    [SerializeField] private Clue clue;
    [SerializeField] private string[] choices;
    [SerializeField] private int correctChoice;
    [SerializeField] private string[] wrongDialogue;
    
    /// <summary>
    /// Get the text for the dialogue
    /// </summary>
    /// <returns>Dialogue</returns>
    public string[] GetDialogue() {
        return dialogue;
    }

    /// <summary>
    /// Get the question being asked
    /// </summary>
    /// <returns>The question</returns>
    public string GetQuestion() {
        return question;
    }

    /// <summary>
    /// Get the Clue that is to be selected
    /// </summary>
    /// <returns>Clue to be selected</returns>
    public Clue GetClue() {
        return clue;
    }

    /// <summary>
    /// Get the choices that can be selected
    /// </summary>
    /// <returns>Choices that can be selected</returns>
    public string[] GetChoices() {
        return choices;
    }

    /// <summary>
    /// Get the index of the correct choice
    /// </summary>
    /// <returns>The correct choice's index</returns>
    public int GetCorrectChoice() {
        return correctChoice;
    }

    /// <summary>
    /// Get the dialogue when the player chose the wrong answer
    /// </summary>
    /// <returns>Dialogue for selecting the wrong answer</returns>
    public string[] GetWrongDialogue() {
        return wrongDialogue;
    }
}
