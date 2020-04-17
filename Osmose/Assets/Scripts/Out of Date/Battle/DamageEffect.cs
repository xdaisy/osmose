using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour {
    public Text DamageText;
    public float Duration = 1f;
    public float MoveSpeed = 1f;
    public float PlacementJitter = 0.5f;

    public Color DamageColor;
    public Color HealColor;

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, Duration);
        DamageText.rectTransform.position += new Vector3(0f, MoveSpeed * Time.deltaTime, 0f);
    }

    public void SetDamage(Vector3 pos, int damage, bool isAttack) {
        DamageText.text = "" + damage;
        if (isAttack) {
            DamageText.color = DamageColor;
        } else {
            DamageText.color = HealColor;
        }
        DamageText.rectTransform.position = pos;
        DamageText.rectTransform.position += new Vector3(Random.Range(-PlacementJitter, PlacementJitter), Random.Range(-PlacementJitter, PlacementJitter), 0f);
    }
}
