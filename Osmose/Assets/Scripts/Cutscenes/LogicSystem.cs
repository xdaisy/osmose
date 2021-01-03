/// <summary>
/// Class for the Logic System logic
/// </summary>
public class LogicSystem {
    /// <summary>
    /// Determine if the step is selecting a clue
    /// </summary>
    /// <param name="logicStep">Step of the Logic System</param>
    /// <returns>True if is selecting a clue, false otherwise</returns>
    public static bool IsSelectingClue(LogicStep logicStep) {
        return logicStep.GetClue() != null;
    }

    /// <summary>
    /// Determine if the player selected the correct Clue
    /// </summary>
    /// <param name="selectedClue">Clue that the player selected</param>
    /// <param name="logicStep">Step of the Logic System</param>
    /// <returns>True if the Clue is the correct one, false otherwise</returns>
    public static bool DidSelectCorrectClue(Clue selectedClue, LogicStep logicStep) {
        return logicStep.GetClue().IsEqual(selectedClue);
    }

    /// <summary>
    /// Determine if the step is selecting multiple choice
    /// </summary>
    /// <param name="logicStep">Step of the Logic System</param>
    /// <returns>True if is selecting multiple choice, false otherwise</returns>
    public static bool IsMultipleChoice(LogicStep logicStep) {
        return logicStep.GetChoices().Length > 0;
    }

    /// <summary>
    /// Determine if the player selected the correct choice
    /// </summary>
    /// <param name="choice">Choice that the player selected</param>
    /// <param name="logicStep">Step of the Logic System</param>
    /// <returns>True if the choice is the correct one, false otherwise</returns>
    public static bool DidSelectCorrectChoice(int choice, LogicStep logicStep) {
        return choice == logicStep.GetCorrectChoice();
    }
}
