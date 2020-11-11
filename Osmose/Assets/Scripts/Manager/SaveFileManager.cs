using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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

        // clues
        public string CurrentChapter;
        public List<string> CurrentClues;
        public List<string> ArenPrologue;
        public List<string> ArenChapter1;
        public List<string> ArenChapter2;
        public List<string> ReyPrologue;
        public List<string> ReyChapter1;
        public List<string> ReyChapter2;
        public List<string> NaoisePrologue;
        public List<string> NaoiseChapter1;
        public List<string> NaoiseChapter2;
        public List<string> Chapter3;
        public List<string> Chapter4;
        public List<string> Chapter5;
        public List<string> Chapter6;
        public List<string> Chapter7;

        // events
        public List<string> Events;

        // clues obtained
        public bool[] ObtainedClues;
        public bool[] UpdatedClues;
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
            XPosition = PlayerControls.Instance.transform.position.x,
            YPosition = PlayerControls.Instance.transform.position.y,
            ZPosition = PlayerControls.Instance.transform.position.z,
            CurrentParty = GameManager.Instance.GetCurrentParty(),
            CurrentChapter = GameManager.Instance.GetCurrentChapter(),
            CurrentClues = GameManager.Instance.GetCurrentClues(),
            ArenPrologue = null,
            ArenChapter1 = null,
            ArenChapter2 = null,
            ReyPrologue = null,
            ReyChapter1 = null,
            ReyChapter2 = null,
            NaoisePrologue = null,
            NaoiseChapter1 = null,
            NaoiseChapter2 = null,
            Chapter3 = null,
            Chapter4 = null,
            Chapter5 = null,
            Chapter6 = null,
            Chapter7 = null,
            Events = EventManager.Instance.GetEvents(),
            ObtainedClues = CluesManager.Instance.GetObtainedClues(),
            UpdatedClues = CluesManager.Instance.GetUpdatedClues()
        };

        // set chapter clues
        List<string> previousChapters = GameManager.Instance.GetPastChapters();
        if (previousChapters.IndexOf(Constants.AREN_PROLOGUE) > -1) {
            saveFiles[file].ArenPrologue = GameManager.Instance.GetChapterClues(Constants.AREN_PROLOGUE);
        }
        if (previousChapters.IndexOf(Constants.AREN_CHAPER_1) > -1) {
            saveFiles[file].ArenChapter1 = GameManager.Instance.GetChapterClues(Constants.AREN_CHAPER_1);
        }
        if (previousChapters.IndexOf(Constants.AREN_CHAPER_2) > -1) {
            saveFiles[file].ArenChapter2 = GameManager.Instance.GetChapterClues(Constants.AREN_CHAPER_2);
        }
        if (previousChapters.IndexOf(Constants.REY_PROLOGUE) > -1) {
            saveFiles[file].ReyPrologue = GameManager.Instance.GetChapterClues(Constants.REY_PROLOGUE);
        }
        if (previousChapters.IndexOf(Constants.REY_CHAPER_1) > -1) {
            saveFiles[file].ReyChapter1 = GameManager.Instance.GetChapterClues(Constants.REY_CHAPER_1);
        }
        if (previousChapters.IndexOf(Constants.REY_CHAPER_2) > -1) {
            saveFiles[file].ReyChapter2 = GameManager.Instance.GetChapterClues(Constants.REY_CHAPER_2);
        }
        if (previousChapters.IndexOf(Constants.NAOISE_PROLOGUE) > -1) {
            saveFiles[file].NaoisePrologue = GameManager.Instance.GetChapterClues(Constants.NAOISE_PROLOGUE);
        }
        if (previousChapters.IndexOf(Constants.NAOISE_CHAPER_1) > -1) {
            saveFiles[file].NaoiseChapter1 = GameManager.Instance.GetChapterClues(Constants.NAOISE_CHAPER_1);
        }
        if (previousChapters.IndexOf(Constants.NAOISE_CHAPER_2) > -1) {
            saveFiles[file].NaoiseChapter2 = GameManager.Instance.GetChapterClues(Constants.NAOISE_CHAPER_2);
        }
        if (previousChapters.IndexOf(Constants.CHAPER_3) > -1) {
            saveFiles[file].Chapter3 = GameManager.Instance.GetChapterClues(Constants.CHAPER_3);
        }
        if (previousChapters.IndexOf(Constants.CHAPER_4) > -1) {
            saveFiles[file].Chapter4 = GameManager.Instance.GetChapterClues(Constants.CHAPER_4);
        }
        if (previousChapters.IndexOf(Constants.CHAPER_5) > -1) {
            saveFiles[file].Chapter5 = GameManager.Instance.GetChapterClues(Constants.CHAPER_5);
        }
        if (previousChapters.IndexOf(Constants.CHAPER_6) > -1) {
            saveFiles[file].Chapter6 = GameManager.Instance.GetChapterClues(Constants.CHAPER_6);
        }
        if (previousChapters.IndexOf(Constants.CHAPER_7) > -1) {
            saveFiles[file].Chapter7 = GameManager.Instance.GetChapterClues(Constants.CHAPER_7);
        }

        // set lastMove
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
            Vector2 lastMove = new Vector2(saveFiles[file].LastMoveX, saveFiles[file].LastMoveY);
            PlayerControls.Instance.SetLastMove(lastMove);
            Vector3 playerPos = new Vector3(saveFiles[file].XPosition, saveFiles[file].YPosition, saveFiles[file].ZPosition);
            PlayerControls.Instance.SetPosition(playerPos);

            // load party
            GameManager.Instance.ChangeMembers(saveFiles[file].CurrentParty);

            // load clues
            GameManager.Instance.SetCurrentChapter(saveFiles[file].CurrentChapter);
            GameManager.Instance.SetCurrentClues(saveFiles[file].CurrentClues);
            GameManager.Instance.SetChapterClues(Constants.AREN_PROLOGUE, saveFiles[file].ArenPrologue);
            GameManager.Instance.SetChapterClues(Constants.AREN_CHAPER_1, saveFiles[file].ArenChapter1);
            GameManager.Instance.SetChapterClues(Constants.AREN_CHAPER_2, saveFiles[file].ArenChapter2);
            GameManager.Instance.SetChapterClues(Constants.REY_PROLOGUE, saveFiles[file].ReyPrologue);
            GameManager.Instance.SetChapterClues(Constants.REY_CHAPER_1, saveFiles[file].ReyChapter1);
            GameManager.Instance.SetChapterClues(Constants.REY_CHAPER_2, saveFiles[file].ReyChapter2);
            GameManager.Instance.SetChapterClues(Constants.NAOISE_PROLOGUE, saveFiles[file].NaoisePrologue);
            GameManager.Instance.SetChapterClues(Constants.NAOISE_CHAPER_1, saveFiles[file].NaoiseChapter1);
            GameManager.Instance.SetChapterClues(Constants.NAOISE_CHAPER_2, saveFiles[file].NaoiseChapter2);
            GameManager.Instance.SetChapterClues(Constants.CHAPER_3, saveFiles[file].Chapter3);
            GameManager.Instance.SetChapterClues(Constants.CHAPER_4, saveFiles[file].Chapter4);
            GameManager.Instance.SetChapterClues(Constants.CHAPER_5, saveFiles[file].Chapter5);
            GameManager.Instance.SetChapterClues(Constants.CHAPER_6, saveFiles[file].Chapter6);
            GameManager.Instance.SetChapterClues(Constants.CHAPER_7, saveFiles[file].Chapter7);

            // load events
            EventManager.Instance.LoadEvents(saveFiles[file].Events);

            // load obtained clues
            CluesManager.Instance.LoadObtainedClues(saveFiles[file].ObtainedClues);

            // load updated clues
            CluesManager.Instance.LoadUpdatedClues(saveFiles[file].UpdatedClues);

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
