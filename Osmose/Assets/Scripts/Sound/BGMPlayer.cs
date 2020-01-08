using UnityEngine;

/// <summary>
/// A class that plays the BGM assigned to the scene
/// </summary>
public class BGMPlayer : MonoBehaviour {
    public int BgmTrack;

    // Start is called before the first frame update
    void Start() {
        SoundManager.Instance.PlayBGM(BgmTrack);
    }
}
