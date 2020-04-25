using UnityEngine;

public class EssentialsLoader : MonoBehaviour {
    public GameObject PlayerLoader;
    public GameObject GameMang;
    public GameObject UICanvas;
    public GameObject LoadEntrance;
    public GameObject SceneLoader;

    private void Awake() {
        if (PlayerControls.Instance == null) {
            Instantiate(PlayerLoader);
        }

        if (GameManager.Instance == null) {
            Instantiate(GameMang);
        }

        if (UIFade.Instance == null) {
            Instantiate(UICanvas);
        }

        if (LoadSceneLogic.Instance == null) {
            Instantiate(SceneLoader);
        }

        Instantiate(LoadEntrance);
    }
}
