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

    private int enemyID = 0; // ID indicating which enemy it is if have multiple of same enemy
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

    /// <summary>
    /// Play or stop selected animation
    /// </summary>
    /// <param name="selected">Flag indicating whether or not the enemy is selected</param>
    public void Highlight(bool selected) {
        anim.SetBool("IsSelected", selected);
    }

    private IEnumerator playDeathAnimation() {
        anim.SetTrigger("Died");
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Play the enemy's move animation
    /// </summary>
    public void DoMove() {
        StartCoroutine(playMoveAnimation());
    }

    private IEnumerator playMoveAnimation() {
        anim.SetBool("DoMove", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("DoMove", false);
    }

    /// <summary>
    /// Set the Enemy's ID
    /// </summary>
    /// <param name="id">ID of the enemy</param>
    public void SetEnemyID(int id) {
        this.enemyID = id;
    }

    /// <summary>
    /// Get the enemy's display name
    /// </summary>
    /// <returns>Enemy's display name</returns>
    public string GetName() {
        return this.enemyID > 1 ? this.EnemyName + " " + this.enemyID : this.EnemyName;
    }
}
