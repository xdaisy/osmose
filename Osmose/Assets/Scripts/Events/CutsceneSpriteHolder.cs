using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneSpriteHolder : MonoBehaviour {

    public List<Sprite> Sprites;
    private Dictionary<string, Sprite> sprites;

	// Use this for initialization
	void Start () {
        
    }

    private void Awake() {
        sprites = new Dictionary<string, Sprite>();
        foreach (Sprite sprite in Sprites) {
            sprites[sprite.name] = sprite;
        }
    }

    public Sprite GetSprite(string name) {
        return sprites[name];
    }
}
