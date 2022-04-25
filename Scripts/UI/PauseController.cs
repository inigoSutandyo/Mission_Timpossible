using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    private PauseManager pause;

    private void Awake()
    {
        pause = FindObjectOfType<PauseManager>();
    }

    public void PressContinue()
    {
        pause.ContinueGame();
    }

    public void PressQuit()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
