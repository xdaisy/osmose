using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyHud : MonoBehaviour{
    public Text[] PartyNames;
    public Text[] PartyHP;
    public Text[] PartySP;
    public Image[] PartyImages;

    private List<string> currParty;
    private int numActiveUI;

    // Start is called before the first frame update
    void Start() {
        numActiveUI = 0;
        currParty = GameManager.Instance.Party.GetCurrentParty();
        for (int i = 0; i < PartyNames.Length; i++) {
            if (i >= currParty.Count) {
                // party member isn't active
                PartyImages[i].gameObject.SetActive(false);
            } else {
                // party member active
                PartyNames[i].text = currParty[i];
                PartyHP[i].text = "" + GameManager.Instance.Party.GetCharCurrHP(currParty[i]) + "/" + GameManager.Instance.Party.GetCharMaxHP(currParty[i]);
                PartySP[i].text = "" + GameManager.Instance.Party.GetCharCurrSP(currParty[i]) + "/" + GameManager.Instance.Party.GetCharMaxSP(currParty[i]);
                PartyImages[i].gameObject.SetActive(true);
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
                PartyHP[i].text = "" + GameManager.Instance.Party.GetCharCurrHP(currParty[i]) + "/" + GameManager.Instance.Party.GetCharMaxHP(currParty[i]);
                PartySP[i].text = "" + GameManager.Instance.Party.GetCharCurrSP(currParty[i]) + "/" + GameManager.Instance.Party.GetCharMaxSP(currParty[i]);
            }
        }
    }

    public int GetNumActiveUI() {
        return numActiveUI;
    }
}
