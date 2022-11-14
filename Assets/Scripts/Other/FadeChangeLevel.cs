using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeChangeLevel : MonoBehaviour
{
  public Animator animator;
  private int sceneLoaded;
  void Update()
  {
    if (Input.GetMouseButton(0))
    {
      //Commented out as it made it impossible to exit any menus
      //FadeToScene(0);
    }
  }

  public void ChangeLevel(int sceneIndex)
  {
    SceneManager.LoadScene(sceneLoaded);
  }
  public void FadeToScene(int sceneIndex)
  {
    sceneLoaded = sceneIndex;
    animator.SetTrigger("FadeOut");
  }
}
