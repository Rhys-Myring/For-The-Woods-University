/*
Purpose: Loads the first level
Author:  Ilyes Benachi
Date:    11/08/2021
Notes:   This script ends the current scene and loads the first level of the game
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCurrentLevel : MonoBehaviour
{
    //Variables to store the current scene and it's name
    private Scene currentScene;

    /* Loads the scene of the first
   it does this by getting the name of the first level */
    public void ButtonChangeScene(string sceneName)
    {
        Time.timeScale = 1f;
        GameObject.Find("LevelChanger").GetComponent<Animator>().SetTrigger("FadeOut");
        
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLastScene()
    {
        Time.timeScale = 1f;
        currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}