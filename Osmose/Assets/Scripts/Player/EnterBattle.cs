using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterBattle : MonoBehaviour
{
    public float WaitToLoad = 1f;
    public float waitTimer;

    private Vector2 prevPos; // keep track of previous pos

    private float amountPlayerMoved; // keep track how much the player has moved
    private float movementTilEncounter; // amount of movement before player launch into battles

    private bool shouldLoadAfterFade;

    // Start is called before the first frame update
    void Start()
    {
        amountPlayerMoved = 0f;
        waitTimer = WaitToLoad;
        movementTilEncounter = Random.Range(10f, 25f);
        prevPos = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsBattleMap) {
            if (shouldLoadAfterFade) {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f) {
                    shouldLoadAfterFade = false;
                    SceneManager.LoadScene("Battle");
                }
            } else if (!shouldLoadAfterFade && amountPlayerMoved >= movementTilEncounter) {
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

    public void goIntoBattle() {
        shouldLoadAfterFade = true;
        UIFade.Instance.FadeToBlack();
        GameManager.Instance.FadingBetweenAreas = true;
        waitTimer = WaitToLoad;
    }

    public void UpdatePos() {
        prevPos = new Vector2(transform.position.x, transform.position.y);
    }
}
