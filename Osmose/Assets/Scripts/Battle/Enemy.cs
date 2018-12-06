using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float CurrentHP;
    public float MaxHP;
    public float CurrentSP;
    public float MaxSP;
    public float Attack;
    public float Defense;
    public float MagicDefense;
    public float Speed;
    public float Luck;

    public int Exp; // amount of exp it gives
    public int Money; // amount of money it gives

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (CurrentHP <= 0) {
            Destroy(this.gameObject);
        }
	}
}
