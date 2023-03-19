using UnityEngine;

/// <summary>
/// A class that plays the BGM assigned to the scene
/// </summary>
public class BGMPlayer : MonoBehaviour {
    public int BgmTrack;
    public SceneName SceneName;
    public int PostEventBGMTrack;

    // Start is called before the first frame update
    void Start() {
        if (SceneName != null && EventManager.Instance.DidEventHappened(SceneName.GetSceneName())) {
            SoundManager.Instance.PlayBGM(PostEventBGMTrack);
        }
        else {
            SoundManager.Instance.PlayBGM(BgmTrack);
        }
    }
}
