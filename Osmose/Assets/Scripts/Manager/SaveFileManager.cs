using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;

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

    public static void Load() {
        string path = "Assets/Resources/save.txt";

        StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();

        SaveData save = JsonUtility.FromJson<SaveData>(json);

        GameManager.Instance.CurrentScene = save.CurrentScene;
        GameManager.Instance.IsBattleMap = save.IsBattleMap;
        GameManager.Instance.SetMagicMeter(save.MagicMeter);
        Vector2 lastMove = new Vector2(save.LastMoveX, save.LastMoveY);
        PlayerControls.Instance.SetLastMove(lastMove);
        Vector3 playerPos = new Vector3(save.XPosition, save.YPosition, save.ZPosition);
        PlayerControls.Instance.SetPosition(playerPos);
        GameManager.Instance.Wallet = save.Wallet;
        GameManager.Instance.ItemsHeld = new List<string>(save.ItemsHeld);
        GameManager.Instance.NumOfItems = new List<int>(save.NumOfItems);
        GameManager.Instance.EquipmentHeld = new List<string>(save.EquipmentHeld);
        GameManager.Instance.NumOfEquipment = new List<int>(save.NumOfEquipment);
        GameManager.Instance.EquipmentHeld = new List<string>(save.KeyItemsHeld);
        GameManager.Instance.Party.LoadCharStats("Aren", save.Aren);
        GameManager.Instance.Party.LoadCharStats("Rey", save.Rey);
        GameManager.Instance.Party.LoadCharStats("Naoise", save.Naoise);
        save.OpenedChest.CopyTo(ObtainItemManager.Instance.OpenedChest, 0);
        save.PickedUpItem.CopyTo(ObtainItemManager.Instance.PickedUpItem, 0);

        SceneManager.LoadScene(GameManager.Instance.CurrentScene);
    }
}
