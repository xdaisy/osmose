using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager {

    private static HashSet<string> events = new HashSet<string>(); // set of all the events that occurred

    public static void AddEvent(string scene) {
        // added the cutscene that happened
        events.Add(scene);
    }

    public static bool DidEventHappened(string scene) {
        return events.Contains(scene); // cutscene happened if cutscenes contains it
    }
}
