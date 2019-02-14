using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMenu : MonoBehaviour
{
    [Header("Character UI")]
    public CanvasGroup SelectPanel;
    public GameObject[] CharSelection;
    public Image[] CharImages;

    [Header("HP")]
    public Slider[] HPSliders;
    public Text[] HPAmount;

    [Header("SP")]
    public Slider[] SPSliders;
    public Text[] SPAmount;
    private bool updated;
 
    // Start is called before the first frame update
    void Start()
    {
        updated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (SelectPanel.interactable && updated) {
            // only update if on select panel and was updated
            List<string> party = GameManager.Instance.Party.GetCurrentParty();
            for (int i = 0; i < CharSelection.Length; i++) {
                if (i >= party.Count) {
                    // if no other members
                    CharSelection[i].SetActive(false);
                    continue;
                }
                CharSelection[i].SetActive(true);
                string charName = party[i];
                int currHp = GameManager.Instance.Party.GetCharacterCurrentHP(charName);
                int maxHp = GameManager.Instance.Party.GetCharacterMaxHP(charName);
                int currSp = GameManager.Instance.Party.GetCharacterCurrentSP(charName);
                int maxSp = GameManager.Instance.Party.GetCharacterMaxSP(charName);

                switch (charName) {
                    case "Aren":
                        Debug.Log("setting sprite");
                        CharImages[i].sprite = GameManager.Instance.ArenSprite;
                        break;
                    case "Rey":
                        break;
                    case "Naoshe":
                        break;
                }

                HPSliders[i].value = currHp / maxHp;
                HPAmount[i].text = "" + currHp + "/" + maxHp;

                SPSliders[i].value = currSp / maxSp;
                SPAmount[i].text = "" + currSp + "/" + maxSp;
            }

            updated = false;
        }
    }

    public void UpdateSelectPanel() {
        updated = true;
    }
}
