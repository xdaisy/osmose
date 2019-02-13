using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    public string EnemyName;

    private Image EnemyImage; // displayed image of the enemy
    public Sprite EnemySprite; // regular sprite
    public Sprite HighlightedSprite; // sprite of enemy highlighted

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

	// Use this for initialization
	void Start () {
        EnemyImage = this.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if (CurrentHP <= 0) {
            Destroy(this.gameObject);
        }
	}

    public void Highlight(bool selected) {
        if (selected) {
            EnemyImage.sprite = HighlightedSprite;
        } else {
            EnemyImage.sprite = EnemySprite;
        }
    }
}
