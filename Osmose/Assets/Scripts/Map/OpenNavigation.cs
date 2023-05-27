using UnityEngine;

public class OpenNavigation : MonoBehaviour {
    public Navigation NavigationCanvas;
    public string AreaName;
    public SceneName SceneToLoad;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            /*GameManager.Instance.OnMap = true;
            GameManager.Instance.PreviousScene = AreaName;

            NavigationCanvas.Open();*/
            GameManager.Instance.PreviousScene = Constants.MAP;
            GameManager.Instance.CurrentScene = SceneToLoad.GetSceneName();

            GameManager.Instance.OnMap = false;

            LoadSceneLogic.Instance.LoadScene(SceneToLoad.GetSceneName(), Vector2.up);
        }
    }
}
