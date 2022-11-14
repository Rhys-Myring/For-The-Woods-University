using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseResumeGame : MonoBehaviour
{

    //public KartInputManager kartInputManager;
    public static bool GamePaused = false;
    public GameObject pauseUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("PAUSEE");
            if (GamePaused == false /*&& gameState == State.Game*/)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }
    public void PauseGame()
    {
        //Movement.gameState = Movement.State.Pause;
        //kartInputManager.pauseInput = false;
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void ResumeGame()
    {
        //Movement.gameState = Movement.State.Game;
        //kartInputManager.pauseInput = false;
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }
}