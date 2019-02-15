using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyHud : MonoBehaviour{
    public Text[] PartyNames;
    public Text[] PartyHP;
    public Image[] PartyImages;

    private List<string> currParty;
    private int numActiveUI;

    // Start is called before the first frame update
    void Start()
    {
        numActiveUI = 0;
        currParty = GameManager.Instance.Party.GetCurrentParty();
        for (int i = 0; i < PartyNames.Length; i++) {
            if (i >= currParty.Count) {
                // party member isn't active
                PartyNames[i].text = "";
                PartyHP[i].text = "";
                Color color = PartyImages[i].color;
                PartyImages[i].color = new Color(color.r, color.g, color.b, 0);
            } else {
                // party member active
                PartyNames[i].text = currParty[i];
                PartyHP[i].text = "" + GameManager.Instance.Party.GetCharacterCurrentHP(currParty[i]) + "/" + GameManager.Instance.Party.GetCharacterMaxHP(currParty[i]);
                Color color = PartyImages[i].color;
                PartyImages[i].color = new Color(color.r, color.g, color.b, 1);
                numActiveUI++;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        for (int i = 0; i < PartyNames.Length; i++) {
            if (i >= currParty.Count) {
                continue;
            } else {
                PartyHP[i].text = "" + GameManager.Instance.Party.GetCharacterCurrentHP(currParty[i]) + "/" + GameManager.Instance.Party.GetCharacterMaxHP(currParty[i]);
            }
        }
    }

    public int GetNumActiveUI() {
        return numActiveUI;
    }
}
