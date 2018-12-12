using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Overarching game object. It is ever existing. There can be only one, but cannot be accessed from anywhere.
/// </summary>
public class GameInstance : MonoBehaviour
{
    private static GameInstance instance;

    public static GameMode GameMode { get { return instance.currentGameMode; } }


    private GameMode currentGameMode;
    
    
    /// <summary>
    /// MonoBehaviour Awake
    /// </summary>
    private void Awake()
    {
        instance = this;

        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.sceneUnloaded += SceneUnloaded;
    }

    private void SceneUnloaded(Scene arg0)
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        SceneManager.sceneUnloaded -= SceneUnloaded;

        instance = null;
    }


    /// <summary>
    /// Callback to sceneLoaded.
    /// </summary>
    private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        // Whenever the scene is loaded, we must find the gameMode object.
        // It will in turn spawn whatever is needed for this world to function.
        currentGameMode = FindObjectOfType<GameMode>();
        currentGameMode.Init();
    }
}
