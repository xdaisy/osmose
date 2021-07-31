using UnityEngine;
using UnityEngine.EventSystems;

public class GameOver : MonoBehaviour {
    public GameObject YesButton;
    
    // Start is called before the first frame update
    void Start() {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(YesButton);
    }

    public void Yes() {
        LoadSceneLogic.Instance.LoadScene(GameManager.Instance.PreviousScene);
    }

    public void No() {
        LoadSceneLogic.Instance.LoadScene(Constants.MAIN);
    }
}
