using System;
using System.Collections;
using UnityEngine;

public struct EnemyTurn {
    public bool Defend;
    public bool Attack;
    public bool Heal;
    public int Target;
    public int Amount;
}

[Serializable]
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

        EnemyStats stats = StatsManager.Instance.GetEnemyStats(EnemyName);
        CurrentHP = stats.HP;
        MaxHP = stats.HP;
        CurrentSP = stats.SP;
        MaxSP = stats.SP;
        Attack = stats.Attack;
        Defense = stats.Defense;
        MagicDefense = stats.MagicDefense;
        Speed = stats.Speed;
        Luck = stats.Luck;
        Exp = stats.EXP;
        Money = stats.Money;
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
