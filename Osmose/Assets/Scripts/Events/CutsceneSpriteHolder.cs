using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneSpriteHolder : MonoBehaviour {

    public List<Sprite> Sprites;
    private Dictionary<string, Sprite> sprites;

	// Use this for initialization
	void Start () {
        sprites = new Dictionary<string, Sprite>();
        //foreach(Sprite sprite in Sprites) {
        //    sprites[sprite.name] = sprite;
        //    Debug.Log(sprite.name);
        //}

        Sprite[] loadSprites = Resources.LoadAll<Sprite>("Talking_Sprite");
        foreach(Sprite s in loadSprites) {
            sprites[s.name] = s;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Sprite GetSprite(string name) {
        //return sprites[name];
        foreach(Sprite s in Sprites) {
            if (s.name == name) {
                return s;
            }
        }
        return null;
    }
}
