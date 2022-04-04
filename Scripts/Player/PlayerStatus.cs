using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStatus : MonoBehaviour
{
    public int health = 100;
    public Quest quest;
    public GameObject DeathPanel;

    void Start()
    {
        DeathPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Time.timeScale = 0;
            DeathPanel.SetActive(true);
        }
    }

}