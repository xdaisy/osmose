using UnityEngine;

public class OpenNavigation : MonoBehaviour {
    public Navigation NavigationCanvas;
    public string AreaName;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            GameManager.Instance.OnMap = true;
            GameManager.Instance.PreviousScene = AreaName;

            NavigationCanvas.Open();
        }
    }
}
