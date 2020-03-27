using UnityEngine;

public class EnterBattle : MonoBehaviour {

    private Vector2 prevPos; // keep track of previous pos

    private float amountPlayerMoved; // keep track how much the player has moved
    private float movementTilEncounter; // amount of movement before player launch into battles

    // Start is called before the first frame update
    void Start()
    {
        amountPlayerMoved = 0f;
        movementTilEncounter = Random.Range(10f, 25f);
        prevPos = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsBattleMap) {
            if (amountPlayerMoved >= movementTilEncounter) {
                amountPlayerMoved = 0f; // reset the amount that the player has moved
                goIntoBattle();
                movementTilEncounter = Random.Range(10f, 25f); // new amount of movement until random encounter
            } else {
                Vector2 currPos = new Vector2(transform.position.x, transform.position.y);

                amountPlayerMoved += Vector2.Distance(prevPos, currPos);
                prevPos = currPos;
            }
        }
    }

    /// <summary>
    /// Go into battle
    /// </summary>
    public void goIntoBattle() {
        GameManager.Instance.PreviousScene = GameManager.Instance.CurrentScene;
        LoadSceneLogic.Instance.LoadScene(Constants.BATTLE);
    }

    /// <summary>
    /// Update the new position
    /// </summary>
    public void UpdatePos() {
        prevPos = new Vector2(transform.position.x, transform.position.y);
    }
}
