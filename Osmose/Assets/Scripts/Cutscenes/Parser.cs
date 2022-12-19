using System;

public struct Portrait {
    public string name;
    public string spriteName;
    public string animationName;
    public float animationTime;
    public bool hideCG;
    public bool showCG;
    public int newTrack;
};

/// <summary>
/// Class that holds static methods for parsing dialogue text
/// </summary>
public class Parser {
    private static string[] dialogueSeparators = { ":" };

    /// <summary>
    /// Parse the line into the character name and the sprite name
    /// </summary>
    /// <param name="line">Line which holds the name</param>
    /// <returns>Struct that contains the name of the character and the sprite</returns>
    public static Portrait ParsePortrait(string line) {
        string name = "";
        string spriteName = "";

        int indexOfAngBracket = line.IndexOf('<');
        int indexOfParenthesis = line.IndexOf('(');
        if (indexOfAngBracket < 0) {
            indexOfAngBracket = line.Length;
        }
        if (indexOfParenthesis < 0) {
            indexOfParenthesis = line.Length;
        }
        string namePortrait = line.Substring(0, indexOfAngBracket);
        string animationCommand = "";
        if (indexOfAngBracket > -1 && indexOfParenthesis != line.Length) {
            animationCommand = line.Substring(indexOfAngBracket, (indexOfParenthesis - indexOfAngBracket));
        } else {
            animationCommand = line.Substring(indexOfAngBracket);
        }
        string trackIndex = line.Substring(indexOfParenthesis);

        if (namePortrait.Contains("-")) {
            string[] person = namePortrait.Split('-');
            name = person[0];
            spriteName = !isPortraitless(person[1]) ? person[0] + "_" + person[1] : person[1];
        } else {
            // no "-" in line
            if (isPortraitless(namePortrait)) {
                spriteName = namePortrait;
            } else {
                name = namePortrait;
            }
        }

        // get animation information
        string animationName = getAnimationName(animationCommand);
        float animationTime = getAnimationTime(animationCommand);

        // get new track
        int track = getTrackIndex(trackIndex);

        return new Portrait {
            name = name,
            spriteName = spriteName,
            animationName = animationName,
            animationTime = animationTime,
            showCG = animationCommand.Equals(Constants.SHOW_CG),
            hideCG = animationCommand.Equals(Constants.HIDE_CG),
            newTrack = track
        };
    }

    /// <summary>
    /// Place the new line into the string
    /// </summary>
    /// <param name="line">Line of dialogue</param>
    /// <returns>Line of dialogue with new lines if there are any</returns>
    public static string PlaceNewLine(string line) {
        if (line.Contains("\\n")) {
            // put in new lines in dialogue
            return line.Replace("\\n", Environment.NewLine);
        }

        return line;
    }

    /// <summary>
    /// Split the dialogue for the Logic Step into the portrait and the dialogue
    /// </summary>
    /// <param name="dialogue">Dialogue in the Logic Step object</param>
    /// <returns>String array that contains the portrait and the line of dialogue</returns>
    public static string[] SplitLogicDialogue(string dialogue) {
        return dialogue.Split(dialogueSeparators, StringSplitOptions.None);
    }

    /// <summary>
    /// Get whether or not there is a portrait shown
    /// </summary>
    /// <param name="portraitName">Name of the portrait</param>
    /// <returns>True if it is portraitless, false otherwise</returns>
    private static bool isPortraitless(string portraitName) {
        return portraitName.Equals("Portraitless");
    }

    /// <summary>
    /// Get the animation name
    /// </summary>
    /// <param name="animationCommand">Command for the animation</param>
    /// <returns>Animation name if the command exists</returns>
    private static string getAnimationName(string animationCommand) {
        switch(animationCommand) {
            case Constants.JUMP:
                return "Jump";
            case Constants.SHAKE:
                return "Shake";
            default:
                return "";
        }
    }

    /// <summary>
    /// Get the animation time
    /// </summary>
    /// <param name="animationCommand">Command for the animation</param>
    /// <returns>Animation time if the command exists</returns>
    private static float getAnimationTime(string animationCommand) {
        switch (animationCommand) {
            case Constants.JUMP:
                return 0.5f;
            case Constants.SHAKE:
                return 0.25f;
            default:
                return 0f;
        }
    }

    /// <summary>
    /// Get the track index
    /// </summary>
    /// <param name="trackString">String that contains the track index in parenthesis</param>
    /// <returns></returns>
    private static int getTrackIndex(string trackString) {
        if (trackString.Length < 1) {
            return -1;
        }

        int indexOfOpenParent = trackString.IndexOf('(');
        int indexOfEndParent = trackString.IndexOf(')');
        int trackInd = -1;

        if (indexOfOpenParent > -1 && indexOfEndParent > -1) {
            string track = trackString.Substring(indexOfOpenParent + 1, (indexOfEndParent - indexOfOpenParent - 1));
            try {
                trackInd = Int32.Parse(track);
            } catch (FormatException) {
            }

        }

        return trackInd;
    }
}
