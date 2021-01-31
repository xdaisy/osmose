using UnityEngine;
using UnityEngine.EventSystems;

public class GameOver : MonoBehaviour {
    public EventSystem eventSystem;
    public GameObject YesButton;
    
    // Start is called before the first frame update
    void Start() {
        eventSystem.SetSelectedGameObject(YesButton);   
    }

    public void Yes() {
        LoadSceneLogic.Instance.LoadScene(GameManager.Instance.PreviousScene);
    }

    public void No() {
        LoadSceneLogic.Instance.LoadScene(Constants.MAIN);
    }
}
