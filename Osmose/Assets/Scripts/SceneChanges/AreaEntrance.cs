using UnityEngine;

public class AreaEntrance : MonoBehaviour {
    public string transitionFromArea;

    public bool IsBattleMap;

    public bool IsEntrance;

    // Use this for initialization
    void Start() {
        if (transitionFromArea == PlayerControls.Instance.PreviousAreaName || (
            IsEntrance && PlayerControls.Instance.PreviousAreaName == Constants.MAP)) {
            // set player to entrance's position
            PlayerControls.Instance.SetPosition(transform.position);
            GameManager.Instance.IsBattleMap = IsBattleMap;
            if (!IsBattleMap) {
                GameManager.Instance.LastTown = GameManager.Instance.CurrentScene;
            }
            GameManager.Instance.FadingBetweenAreas = false;
        }
        UIFade.Instance.FadeFromBlack();
    }
}
