using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// custom queue for turn order in battle
public class TurnQueue {
    private LinkedList<string> queue;

    public TurnQueue() {
        queue = new LinkedList<string>();
    }
    
    // push to queue
    public void Enqueue(string name) {
        queue.AddLast(name);
    }

    // pop next in queue
    public string Dequeue() {
        string name = queue.First.Value;
        queue.RemoveFirst();
        return name;
    }

    // remove from queue
    public void RemoveFromQueue(string name) {
        queue.Remove(name);
    }

    public int Count {get { return queue.Count; } }
}
