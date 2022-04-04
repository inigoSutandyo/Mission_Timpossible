using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    public void BackToMenu()
    {
        this.gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
}
