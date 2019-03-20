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
 
    // Start is called before the first frame update
    void Start()
    {
        updateSelectMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (SelectPanel.interactable) {
            // only update if on select panel
            updateSelectMenu();
        }
    }

    private void updateSelectMenu() {
        List<string> party = GameManager.Instance.Party.GetCurrentParty();
        for (int i = 0; i < CharSelection.Length; i++) {
            if (i >= party.Count) {
                // if no other members
                CharSelection[i].SetActive(false);
                continue;
            }
            CharSelection[i].SetActive(true);
            string charName = party[i];
            int currHp = GameManager.Instance.Party.GetCharCurrHP(charName);
            int maxHp = GameManager.Instance.Party.GetCharMaxHP(charName);
            int currSp = GameManager.Instance.Party.GetCharCurrSP(charName);
            int maxSp = GameManager.Instance.Party.GetCharMaxSP(charName);

            switch (charName) {
                case "Aren":
                    CharImages[i].sprite = GameManager.Instance.ArenSprite;
                    break;
                case "Rey":
                    CharImages[i].sprite = GameManager.Instance.ReySprite;
                    break;
                case "Naoise":
                    CharImages[i].sprite = GameManager.Instance.NaoiseSprite;
                    break;
            }

            HPSliders[i].value = ((float) currHp) / maxHp;
            HPAmount[i].text = "" + currHp + "/" + maxHp;

            SPSliders[i].value = ((float) currSp) / maxSp;
            SPAmount[i].text = "" + currSp + "/" + maxSp;
        }
    }
}
