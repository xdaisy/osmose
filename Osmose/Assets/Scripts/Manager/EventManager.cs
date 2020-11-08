using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager that keeps track of all the events that the player have triggered
/// </summary>
public class EventManager : MonoBehaviour {
    public static EventManager Instance;
    private HashSet<string> events = new HashSet<string>(); // set of all the events that has triggered

    void Start() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        // this is for debugging purposes
        AddEvent("Moons_Hallow");
        AddEvent("forest");
    }

    /// <summary>
    /// Add the event to the events that have been triggered
    /// </summary>
    /// <param name="scene">Name of the event</param>
    public void AddEvent(string scene) {
        if (scene.Length > 0 && !events.Contains(scene)) {
            // only adds the event if the player hasn't triggered it
            events.Add(scene);
        }
    }

    /// <summary>
    /// Return a flag that indicates whether or not the event has triggered
    /// </summary>
    /// <param name="scene">Name of the event that want to verify if it has triggered</param>
    /// <returns>true if the event has occurred, false otherwise</returns>
    public bool DidEventHappened(string scene) {
        return events.Contains(scene); // cutscene happened if cutscenes contains it
    }

    /// <summary>
    /// Get all the events that has been triggered
    /// </summary>
    /// <returns>Set of all the events that has been triggered</returns>
    public List<string> GetEvents() {
        return new List<string>(events);
    }

    /// <summary>
    /// Load the events from the save data
    /// </summary>
    /// <param name="events">Set of events in the save data</param>
    public void LoadEvents(List<string> events) {
        this.events.UnionWith(events);
    }
}
