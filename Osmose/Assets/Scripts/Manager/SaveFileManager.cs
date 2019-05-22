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
        public float PlayTime;
        public string CurrentScene;
        public string LastTown;
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

        // current party
        public List<string> CurrentParty;

        // character stats
        public CharStats Aren;
        public CharStats Rey;
        public CharStats Naoise;

        // opened chests
        public bool[] OpenedChest;
        public bool[] PickedUpItem;
    }

    private static string path = Path.Combine(Application.dataPath, "Resources/save");

    public static void Save(int file) {
        SaveData save = new SaveData {
            PlayTime = GameManager.Instance.GetPlayTime(),
            CurrentScene = GameManager.Instance.CurrentScene,
            LastTown = GameManager.Instance.LastTown,
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
            CurrentParty = GameManager.Instance.Party.GetCurrentParty(),
            Aren = GameManager.Instance.Party.GetCharacterStats(Constants.AREN),
            Rey = GameManager.Instance.Party.GetCharacterStats(Constants.REY),
            Naoise = GameManager.Instance.Party.GetCharacterStats(Constants.NAOISE),
            OpenedChest = ObtainItemManager.Instance.OpenedChest,
            PickedUpItem = ObtainItemManager.Instance.PickedUpItem
        };
        Vector2 lastMove = PlayerControls.Instance.GetLastMove();
        save.LastMoveX = lastMove.x;
        save.LastMoveY = lastMove.y;

        string json = JsonUtility.ToJson(save);

        string savePath = path + file + ".txt";

        if (SaveExists(file)) {
            File.Delete(savePath);
        }

        StreamWriter writer = new StreamWriter(savePath, false);
        writer.WriteLine(json);
        writer.Close();
    }

    public static void Load(int file) {
        if (SaveExists(file)) {
            // only load if file exists
            string savePath = path + file + ".txt";

            StreamReader reader = new StreamReader(savePath);
            string json = reader.ReadToEnd();

            SaveData save = JsonUtility.FromJson<SaveData>(json);

            GameManager.Instance.SetPlayTIme(save.PlayTime);

            GameManager.Instance.CurrentScene = save.CurrentScene;
            GameManager.Instance.LastTown = save.LastTown;
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

            GameManager.Instance.Party.ChangeMembers(save.CurrentParty);
            GameManager.Instance.Party.LoadCharStats(Constants.AREN, save.Aren);
            GameManager.Instance.Party.LoadCharStats(Constants.REY, save.Rey);
            GameManager.Instance.Party.LoadCharStats(Constants.NAOISE, save.Naoise);

            save.OpenedChest.CopyTo(ObtainItemManager.Instance.OpenedChest, 0);
            save.PickedUpItem.CopyTo(ObtainItemManager.Instance.PickedUpItem, 0);

            PlayerControls.Instance.PreviousAreaName = "Continue";

            SceneManager.LoadScene(GameManager.Instance.CurrentScene);
        }
    }

    /// <summary>
    /// Return whether or not the save file exists
    /// </summary>
    /// <param name="file">Which save file</param>
    /// <returns>true if file exists, false otherwise</returns>
    public static bool SaveExists(int file) {
        string savePath = path + file + ".txt";
        return File.Exists(savePath);
    }

    /// <summary>
    /// Get the data needed for the save menu
    /// </summary>
    /// <param name="file">Save File</param>
    /// <returns></returns>
    public static SaveMenuData GetSaveData(int file) {
        SaveMenuData saveData = new SaveMenuData();
        saveData.Exists = SaveExists(file);
        if (saveData.Exists) {
            string savePath = path + file + ".txt";

            StreamReader reader = new StreamReader(savePath);
            string json = reader.ReadToEnd();

            SaveData save = JsonUtility.FromJson<SaveData>(json);
            saveData.PlayTime = save.PlayTime;
            saveData.Location = save.CurrentScene;
            saveData.CurrentParty = save.CurrentParty;
        }
        return saveData;
    }
}

/// <summary>
/// Struct of the data of which the Save Menu to display the save files
/// </summary>
public struct SaveMenuData {
    public bool Exists;
    public float PlayTime;
    public string Location;
    public List<string> CurrentParty;
}
