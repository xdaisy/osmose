using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    public string EnemyName;

    [Header("Enemy Stats")]
    public int CurrentHP;
    public int MaxHP;
    public int CurrentSP;
    public int MaxSP;
    public int Attack;
    public int Defense;
    public int MagicDefense;
    public int Speed;
    public int Luck;

    public int Exp; // amount of exp it gives
    public int Money; // amount of money it gives

    public bool IsDefending; // whether enemy is defending or not

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (CurrentHP <= 0) {
            StartCoroutine(playDeathAnimation());
        }
    }

    public void Highlight(bool selected) {
        anim.SetBool("IsSelected", selected);
    }

    private IEnumerator playDeathAnimation() {
        anim.SetTrigger("Died");
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    public void DoMove() {
        StartCoroutine(playMoveAnimation());
    }

    private IEnumerator playMoveAnimation() {
        anim.SetBool("DoMove", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("DoMove", false);
    }
}
