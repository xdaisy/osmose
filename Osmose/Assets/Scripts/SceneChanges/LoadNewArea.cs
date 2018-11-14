using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadNewArea : MonoBehaviour {

    public string sceneToLoad;

    public Image fadeScreen;
    public Animator fadeAnim;

    public Vector2 newPlayerPos;

    public bool isBattleMap;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            //SceneManager.LoadScene(sceneToLoad);
            StartCoroutine(Fade(other));
        }
    }

    IEnumerator Fade(Collider2D other) {
        fadeAnim.SetBool("Fade", true);
        yield return new WaitUntil(() => fadeScreen.color.a == 1); // wait until alpha value is one
        other.transform.Translate(new Vector3(newPlayerPos.x - other.transform.position.x, newPlayerPos.y - other.transform.position.y, 0f));
        other.GetComponent<PlayerControls>().isBattleMap = this.isBattleMap;
        SceneManager.LoadScene(sceneToLoad);
    }
}
