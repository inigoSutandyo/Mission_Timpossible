using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    private bool isFullScreen;
    private void Start()
    {
        isFullScreen = true;
        resolutions = Screen.resolutions;
        CreateResolutionDropdown();
    }

    private void CreateResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        foreach (var res in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData() { text = res.width + "x" + res.height + " : " + res.refreshRate });   
        }
    }

    public void ChangeQualityLevel()
    {
        int dropdown = qualityDropdown.value;
        print(dropdown);
        QualitySettings.SetQualityLevel(dropdown, true);
    }

    public void ChangeResolution()
    {
        int dropdown_value = resolutionDropdown.value;
        print(resolutions[dropdown_value].width + "x" + resolutions[dropdown_value].height);
        Screen.SetResolution(resolutions[dropdown_value].width, resolutions[dropdown_value].height, isFullScreen);
    }

    public void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        isFullScreen = Screen.fullScreen;
    }

    public void BackToMenu()
    {
        this.gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
}
