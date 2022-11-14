/*
Purpose: Loads the next level
Author:  Rhys Myring
Date:    09/08/2021
Notes:   This script ends the current level and loads the next one
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    //Variables to store the current scene and it's name
    private Scene currentScene;
    private string currentSceneName;

    //Variables to store level numbers
    private string levelNumber;
    private int levelNumberInt;

    /* Loads the scene of the next level
       it does this by getting the name of the current level and adding 1 to it to get the next one */
    public void ExitLevel()
    {
        //Gets the current scene name
        currentScene = SceneManager.GetActiveScene();
        currentSceneName = currentScene.name;

        //Gets current scene number
        levelNumber = currentSceneName.Substring(currentSceneName.Length - 1);        
        levelNumberInt = Convert.ToInt32(levelNumber);

        levelNumberInt += 1;

        /* Checks whether the scene for the next level exists, if it does then it is loaded,
           if it doesn't, then the Main Menu scene is loaded instead to avoid errors */ 
        if (CheckIfSceneExists("Level " + levelNumberInt))
        {   
            //Loads next scene
            SceneManager.LoadScene("Level " + levelNumberInt);
        }
        else
        {
            //Loads Main Menu scene
            SceneManager.LoadScene("Main Menu");
        }
    }

    //Checks whether a given scene name exists in the project, returns true if this is the case and false if not
    private bool CheckIfSceneExists(string sceneName)
    {
        //Iterates through all scenes in build and checks if their name is the same as the name being checked
        for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCountInBuildSettings; sceneIndex++)
        {
            //Gets path of the scene index being checked
            string sceneBuildPath = SceneUtility.GetScenePathByBuildIndex(sceneIndex);

            //Gets name of scene index being checked
            string sceneBuildName = sceneBuildPath.Substring(sceneBuildPath.LastIndexOf("/") + 1);
            sceneBuildName = sceneBuildName.Split(Convert.ToChar("."))[0];

            if (sceneBuildName == sceneName)
            {
                //Returns true if scene does exist
                return true;
            }
        }

        //Returns false if scene doesn't exist
        return false;
    }
}
