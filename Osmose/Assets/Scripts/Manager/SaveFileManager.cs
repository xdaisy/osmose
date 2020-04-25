using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that handles the save and load system
/// </summary>
public class SaveFileManager {
    private static SaveData[] saveFiles = new SaveData[3];

    [Serializable]
    private class SaveData {
        // game information
        public float PlayTime;
        public string CurrentScene;
        public float MagicMeter;
        public float LastMoveX;
        public float LastMoveY;
        public float XPosition;
        public float YPosition;
        public float ZPosition;

        // current party
        public List<string> CurrentParty;

        // events
        public List<string> Events;
    }

    // path for unity editor
    private static string editorPath = Application.dataPath + "/Resources/saveFiles.gd";

    // path for build
    private static string buildPath = Application.persistentDataPath + "/saveFiles.gd";

    /// <summary>
    /// Save the file data
    /// </summary>
    /// <param name="file">File being saved to</param>
    public static void Save(int file) {
        saveFiles[file] = new SaveData {
            PlayTime = GameManager.Instance.GetPlayTime(),
            CurrentScene = GameManager.Instance.CurrentScene,
            MagicMeter = GameManager.Instance.GetMagicMeter(),
            XPosition = PlayerControls.Instance.transform.position.x,
            YPosition = PlayerControls.Instance.transform.position.y,
            ZPosition = PlayerControls.Instance.transform.position.z,
            CurrentParty = GameManager.Instance.GetCurrentParty(),
            Events = EventManager.Instance.GetEvents()
        };
        Vector2 lastMove = PlayerControls.Instance.GetLastMove();
        saveFiles[file].LastMoveX = lastMove.x;
        saveFiles[file].LastMoveY = lastMove.y;

        string path = getPath();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(path);
        bf.Serialize(fs, saveFiles);
        fs.Close();
    }

    /// <summary>
    /// Get the save files
    /// </summary>
    public static void LoadSaves() {
        string path = getPath();

        if (File.Exists(path)) {
            // if file exists load files
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(path, FileMode.Open);
            saveFiles = (SaveData[])bf.Deserialize(fs);
            fs.Close();
        }
    }

    private static string getPath() {
        string path = "";
#if UNITY_EDITOR
        path = editorPath;
#endif
#if UNITY_STANDALONE
        path = buildPath;
#endif
        return path;
    }

    /// <summary>
    /// Load the save file
    /// </summary>
    /// <param name="file">The file that is being loaded</param>
    public static void Load(int file) {
        if (saveFiles[file] != null) {
            // set play time
            GameManager.Instance.SetPlayTIme(saveFiles[file].PlayTime);

            // load current location information
            GameManager.Instance.CurrentScene = saveFiles[file].CurrentScene;
            GameManager.Instance.SetMagicMeter(saveFiles[file].MagicMeter);
            Vector2 lastMove = new Vector2(saveFiles[file].LastMoveX, saveFiles[file].LastMoveY);
            PlayerControls.Instance.SetLastMove(lastMove);
            Vector3 playerPos = new Vector3(saveFiles[file].XPosition, saveFiles[file].YPosition, saveFiles[file].ZPosition);
            PlayerControls.Instance.SetPosition(playerPos);

            // load stats
            GameManager.Instance.ChangeMembers(saveFiles[file].CurrentParty);

            // load events
            EventManager.Instance.LoadEvents(saveFiles[file].Events);

            GameManager.Instance.PreviousScene = "Continue";

            LoadSceneLogic.Instance.LoadScene(GameManager.Instance.CurrentScene);
        }
    }

    /// <summary>
    /// Return whether or not the save file exists
    /// </summary>
    /// <param name="file">Which save file</param>
    /// <returns>true if file exists, false otherwise</returns>
    public static bool SaveExists(int file) {
        return saveFiles[file] != null;
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
            saveData.PlayTime = saveFiles[file].PlayTime;
            saveData.Location = saveFiles[file].CurrentScene;
            saveData.CurrentParty = saveFiles[file].CurrentParty;
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
