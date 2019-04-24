using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveFileManager {
    [Serializable]
    private class SaveData {
        // game information
        public string CurrentScene;
        public bool IsBattleMap;
        public float MagicMeter;
        public float LastMoveX;
        public float LastMoveY;
        public float XPosition;
        public float YPosition;
        public float ZPosition;

        // inventory
        public int Wallet;

        public List<string> ItemsHeld;
        public List<int> NumOfItems;

        public List<string> EquipmentHeld;
        public List<int> NumOfEquipment;

        public List<string> KeyItemsHeld;

        // character stats
        public CharStats Aren;
        public CharStats Rey;
        public CharStats Naoise;

        // opened chests
        public bool[] OpenedChest;
        public bool[] PickedUpItem;
    }

    public static void Save() {
        SaveData save = new SaveData {
            CurrentScene = GameManager.Instance.CurrentScene,
            IsBattleMap = GameManager.Instance.IsBattleMap,
            MagicMeter = GameManager.Instance.GetMagicMeter(),
            XPosition = PlayerControls.Instance.transform.position.x,
            YPosition = PlayerControls.Instance.transform.position.y,
            ZPosition = PlayerControls.Instance.transform.position.z,
            Wallet = GameManager.Instance.Wallet,
            ItemsHeld = GameManager.Instance.ItemsHeld,
            NumOfItems = GameManager.Instance.NumOfItems,
            EquipmentHeld = GameManager.Instance.EquipmentHeld,
            NumOfEquipment = GameManager.Instance.NumOfEquipment,
            KeyItemsHeld = GameManager.Instance.KeyItemsHeld,
            Aren = GameManager.Instance.Party.GetCharacterStats("Aren"),
            Rey = GameManager.Instance.Party.GetCharacterStats("Rey"),
            Naoise = GameManager.Instance.Party.GetCharacterStats("Naoise"),
            OpenedChest = ObtainItemManager.Instance.OpenedChest,
            PickedUpItem = ObtainItemManager.Instance.PickedUpItem
        };
        Vector2 lastMove = PlayerControls.Instance.GetLastMove();
        save.LastMoveX = lastMove.x;
        save.LastMoveY = lastMove.y;

        string json = JsonUtility.ToJson(save);

        string path = "Assets/Resources/save.txt";

        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(json);
        writer.Close();
    }
}
