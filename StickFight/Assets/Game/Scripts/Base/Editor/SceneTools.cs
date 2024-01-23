using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SceneTools {
    private const string  FILE_DATA_NAME = "GameData.dat";
    #region Editor Scenes Menu
    public static string[] ScenePaths = { "Assets/Game/Scenes/Logo.unity",
                                          "Assets/Game/Scenes/Home.unity",
                                          "Assets/Game/Scenes/Game.unity",
                                          "Assets/Game/Scenes/Edit.unity",};

    [MenuItem("Game/Play %K", false, 0)]
    private static void PlayGame() {
        OpenLogoScene();
        EditorApplication.isPlaying = true;
    }

    [MenuItem("Game/Scenes/Logo Scene %L", false, 1)]
    private static void OpenLogoScene() {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(ScenePaths[0]);
    }

    [MenuItem("Game/Scenes/Home Scene %H", false, 1)]
    private static void OpenSplashScene() {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(ScenePaths[1]);
    }

    [MenuItem("Game/Scenes/Game Scene %G", false, 1)]
    private static void OpenGamePlayScene() {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(ScenePaths[2]);
    }

    [MenuItem("Game/Scenes/Game Edit %E", false, 1)]
    private static void OpenEditScene() {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(ScenePaths[3]);
    }


    [MenuItem("Game/Delete Data", false, 1)]
    private static void ClearData() {
        //PlayerPrefs.SetString(PLAYER_KEY, JsonUtility.ToJson(playerData));
        string data = JsonUtility.ToJson(new PlayerData());
        //if(!Directory.Exists(FILE_DATA_PATH)) {
        //    Debug.LogError("CreateFile");
        //    Directory.CreateDirectory(FILE_DATA_PATH);
        //}

        try {
            using(StreamWriter writer = File.CreateText(Path.Combine(Application.persistentDataPath, FILE_DATA_NAME))) {
                writer.Write(data);
                Debug.Log($"[DATA] Write file completed.\n <path>: {Path.Combine(Application.persistentDataPath, FILE_DATA_NAME)}\n <content>: {data}");
                writer.Close();
            }
        } catch(Exception e) {
            Debug.LogError($"[DATA] Write file failed.\n <path>: {Path.Combine(Application.persistentDataPath, FILE_DATA_NAME)}\n <error>: {e}");
        }
    }
    #endregion
}
