using UnityEngine;

public class ObtainItemManager : MonoBehaviour {
    public static ObtainItemManager Instance;

    public bool[] OpenedChest;
    public bool[] PickedUpItem;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void OpenChest(int indx) {
        OpenedChest[indx] = true;
    }

    public void PickUpItem(int indx) {
        PickedUpItem[indx] = true;
    }

    public bool DidOpenChest(int indx) {
        return OpenedChest[indx];
    }

    public bool DidPickUpItem(int indx) {
        return PickedUpItem[indx];
    }
}
