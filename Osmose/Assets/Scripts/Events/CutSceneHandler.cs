using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CutSceneHandler {

    private static HashSet<string> events = new HashSet<string>(); // dictionary of all the cutscenes that occurred

    public static void addEvent(string scene) {
        // added the cutscene that happened
        events.Add(scene);
    }

    public static bool didEventHappened(string scene) {
        return events.Contains(scene); // cutscene happened if cutscenes contains it
    }
}
