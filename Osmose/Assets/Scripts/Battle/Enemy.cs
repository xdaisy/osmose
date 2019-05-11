using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct EnemyTurn {
    public bool Defend;
    public bool Attack;
    public bool Heal;
    public int Target;
    public int Amount;
}
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

    public EnemyTurn EnemyDecide(List<int> hostilityMeter) {
        EnemyTurn enemyTurn = new EnemyTurn();

        int move = UnityEngine.Random.Range(0, 5);
        switch (move) {
            case 0:
                // enemy defend
                this.IsDefending = true;

                enemyTurn.Defend = true;
                enemyTurn.Attack = false;
                enemyTurn.Heal = false;
                break;
            default:
                // enemy attack
                List<string> party = GameManager.Instance.Party.GetCurrentParty();

                int target = UnityEngine.Random.Range(0, hostilityMeter.Count);
                string partyMember = party[hostilityMeter[target]];

                int damage = this.Attack - GameManager.Instance.Party.GetCharDef(partyMember);
                if (GameManager.Instance.Party.IsDefending(partyMember)) {
                    damage /= 2;
                }
                damage = Math.Max(damage, 1); // at least do 1 damage
                GameManager.Instance.Party.DealtDamage(partyMember, damage);

                enemyTurn.Attack = true;
                enemyTurn.Defend = false;
                enemyTurn.Heal = false;
                enemyTurn.Target = target;
                enemyTurn.Amount = damage;

                break;
        }
        return enemyTurn;
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
