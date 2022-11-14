using UnityEngine;
using System.Collections;

// Quits the game when the user click on the leaf "Quit"

public class QuitGame : MonoBehaviour
{
  public void ExitGame()
	{
    Application.Quit();
  }
}