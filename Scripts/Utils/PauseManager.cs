using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPause = false;
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private Canvas mainCanvas;

    [SerializeField] PlayerStatus playerStatus;
    public bool isTimeline = false;
    void Start()
    {
        pauseCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerStatus.health <= 0)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isPause)
        {
            PauseGame();
        } 
        
    }
    
    public void PauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        isPause = true;
        pauseCanvas.gameObject.SetActive(true);
        mainCanvas.gameObject.SetActive(false);
    }

    public void ContinueGame()
    {
        if (!isPause) return;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1;
        isPause = false;
        isTimeline = true;
        pauseCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
    }

    public void PauseForTimeline()
    {
        isPause = true;
        mainCanvas.gameObject.SetActive(false);
    }

    public void ContinueForTimeline()
    {
        if (!isPause) return;
        isPause = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        isTimeline = false;
        mainCanvas.gameObject.SetActive(true);
    }
    
}
