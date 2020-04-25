using UnityEngine;

public class AreaEntrance : MonoBehaviour {
    public string transitionFromArea;

    public bool IsEntrance;

    // Use this for initialization
    void Start() {
        if (transitionFromArea == GameManager.Instance.PreviousScene || (
            IsEntrance && GameManager.Instance.PreviousScene == Constants.MAP)) {
            // set player to entrance's position
            PlayerControls.Instance.SetPosition(transform.position);
        }
    }
}
